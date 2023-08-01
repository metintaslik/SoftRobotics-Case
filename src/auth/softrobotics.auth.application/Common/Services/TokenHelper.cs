using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using softrobotics.auth.application.Common.Interface;
using softrobotics.auth.application.Common.Model;
using softrobotics.auth.domain.Entity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace softrobotics.auth.application.Common.Services;

public class TokenHelper : ITokenHelper
{
    private readonly IConfiguration configuration;

    public TokenHelper(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    public TokenDto CreateToken(User user)
    {
        List<Claim> claims = new()
        {
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Email, user.Mail)
        };

        JwtSettings jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>();
        SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(Convert.ToBase64String(Encoding.UTF8.GetBytes(jwtSettings.SecretKey))));
        SigningCredentials signingCredentials = new(key, SecurityAlgorithms.HmacSha256);

        SecurityTokenDescriptor tokenDescriptor = new()
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(jwtSettings.Expiration),
            SigningCredentials = signingCredentials,
            Issuer = jwtSettings.Issuer,
            Audience = jwtSettings.Audience
        };

        JwtSecurityTokenHandler tokenHandler = new();
        SecurityToken securityToken = tokenHandler.CreateToken(tokenDescriptor);
        string token = tokenHandler.WriteToken(securityToken);

        return new TokenDto(token, tokenDescriptor.Expires.Value.TimeOfDay, GenerateRefreshToken());
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];

        using var generator = RandomNumberGenerator.Create();
        generator.GetBytes(randomNumber);

        return Convert.ToBase64String(randomNumber);
    }
}