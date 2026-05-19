using Microsoft.IdentityModel.Tokens;
using ParcialFJCO.Application.Interface;
using ParcialFJCO.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ParcialFJCO.Infraestructure.Service
{
    public class TokenService : ITokenService
    {
        private readonly string _secretKey;
        private readonly int _expiryMinutes;

        public TokenService(string secretKey, int expiryMinutes)
        {
            _secretKey = secretKey;
            _expiryMinutes = expiryMinutes;
        }

        public string GenerateToken(Usuario usuario)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secretKey);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()),
                new Claim("name", usuario.Username),
                new Claim("role", usuario.Role)
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_expiryMinutes),
                Issuer = "ParcialFJCO",
                Audience = "ParcialFJCOUsers",
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
