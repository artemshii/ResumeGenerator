using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ResumeGenerator.Data.Models;
using ResumeGenerator.Data.Interfaces;

namespace ResumeGenerator.Data
{
    public class JWTtokenCreator: IJWTTokenCreator
    {
        private readonly IConfiguration _config;

        public JWTtokenCreator(IConfiguration config)
        {
            _config = config;
        }

        public string CreateToken(string email)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            string JwtToken = _config["JwtToken"] ?? " ";
            var key = System.Text.Encoding.UTF8.GetBytes(JwtToken);

            var claims = new List<Claim>
            {
                new Claim("email", email ?? ""),
                
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(60),
                SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
