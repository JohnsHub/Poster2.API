using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Poster2.API.Models;
using Poster2.API.Models.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Claims;

namespace Poster2.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppUser> _users;
        private readonly SignInManager<AppUser> _signIn;
        private readonly IConfiguration _config;


        public AuthController(UserManager<AppUser> users, SignInManager<AppUser> signIn, IConfiguration config)
        {
            _users = users;
            _signIn = signIn;
            _config = config;
        }

    [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new AppUser { UserName = dto.UserName, Email = dto.Email };
            var res = await _users.CreateAsync(user, dto.Password);
            if (!res.Succeeded) return BadRequest(res.Errors);

            var response = new RegisterResponseDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                DisplayName = dto.DisplayName
            };
            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _users.FindByNameAsync(dto.UserNameOrEmail) ??
                       await _users.FindByEmailAsync(dto.UserNameOrEmail);
            if (user == null) return NotFound("User not found");

            var chk = await _signIn.CheckPasswordSignInAsync(user, dto.Password, false);
            if (!chk.Succeeded) return Unauthorized("Invalid password");

            var jwtSection = _config.GetSection("Jwt");
            var keyBytes = System.Text.Encoding.UTF8.GetBytes(jwtSection["Key"]);
            var creds = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: jwtSection["Issuer"],
                audience: jwtSection["Audience"],
                claims: new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), // Convert Guid to string
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserName)
                },
                expires: DateTime.Now.AddMinutes(2),
                signingCredentials: creds
            );

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo
            });
        }
    }
}
