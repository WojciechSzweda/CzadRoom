using CzadRoom.Models;
using CzadRoom.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CzadRoom.Services
{
    public class JwtTokenManager : IJwtToken {

        private readonly IConfiguration _configuration;

        public JwtTokenManager(IConfiguration configuration) {
            _configuration = configuration;
        }

        public string GenerateToken(User user, DateTime expirationDate) {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var claims = new[] {
                new Claim(ClaimTypes.Name, user.Username)
            };
            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
         _configuration["Jwt:Issuer"],
         expires: expirationDate,
         claims: claims,
         signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
