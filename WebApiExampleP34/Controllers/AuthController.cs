using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApiExampleP34.Models;
using WebApiExampleP34.Models.DTO;

namespace WebApiExampleP34.Controllers;

[Route("api/v1/auth")]
[ApiController]
public class AuthController(UserManager<User> userManager, IConfiguration configuration) : ControllerBase
{
    // register endpoint
    [HttpPost("register")]
    public async Task<AuthResultDto> Register([FromBody] RegisterDto registerDto)
    {
        var existingUser = await userManager.FindByNameAsync(registerDto.Login);
        if (existingUser != null)
        {
            return AuthResultDto.Fail("User already exists");
        }

        var user = new User
        {
            UserName = registerDto.Login,
            Email = registerDto.Login
        };
        var result = await userManager.CreateAsync(user, registerDto.Password);
        if (!result.Succeeded)
        {
            return AuthResultDto.Fail(string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        return AuthResultDto.Ok(GenerateJwtToken(user));
    }

    // login endpoint
    [HttpPost("login")]
    public async Task<AuthResultDto> Login([FromBody] LoginDto loginDto)
    {
        var user = await userManager.FindByNameAsync(loginDto.Login);
        if (user == null || !await userManager.CheckPasswordAsync(user, loginDto.Password))
        {
            return AuthResultDto.Fail("Invalid login or password");
        }
        return AuthResultDto.Ok(GenerateJwtToken(user));
    }

    [HttpGet("profile")]
    [Authorize]
    public UserProfileDto Profile()
    {
        var email = User.Claims.First(x => x.Type == ClaimTypes.Email).Value;
        var id = User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
        return new UserProfileDto
        {
            Id = int.Parse(id),
            Email = email
        };
    }


    private string GenerateJwtToken(User user)
    {
        var key = Encoding.UTF8.GetBytes(configuration["JwtSettings:SecretKey"]!);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Sub, user.Email!),
            new Claim(JwtRegisteredClaimNames.Name, user.Email!),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email!)
        };

        var token = new JwtSecurityToken
        (
            issuer: null,
            audience: null,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(12),
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature
                )
        );

        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.WriteToken(token);
    }
}



