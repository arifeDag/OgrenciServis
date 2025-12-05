using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OgrenciServis.Logic.Interface;
using OgrenciServis.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace OgrenciServis.Logic.Services
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly IConfiguration configuration;

        public JwtTokenService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public string GenerateToken(User user)
        {
            var secretKey = this.configuration["Jwt:SecretKey"];
            var key = Encoding.UTF8.GetBytes(secretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new[]
                {
                    new System.Security.Claims.Claim("UserId",user.UserId.ToString()),
                     new System.Security.Claims.Claim("UserName",user.UserName),
                      new System.Security.Claims.Claim("Role",user.Role),
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                //imzalam 
                SigningCredentials = new SigningCredentials(
                     new SymmetricSecurityKey(key),
                     SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
