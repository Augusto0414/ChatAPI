using ChatAPI.Interfaces;
using ChatAPI.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ChatAPI.Service
{
    public class AuthService : IAuth
    {
        private  readonly IConfiguration _config;

        public AuthService(IConfiguration config)
        {
            _config = config;
        }

        public string GenerateJwtToken(Usuario usuario)
        {
            var key = Convert.FromBase64String(_config["Jwt:Key"]);
            var symmetricKey = new SymmetricSecurityKey(key);
            var creds = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
              new Claim(ClaimTypes.Name, usuario.Name), // Nombre del usuario
              new Claim(ClaimTypes.NameIdentifier, usuario.Id) // UID del usuario
            };

            var token = new JwtSecurityToken(
                claims:  claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public ClaimsPrincipal? ValidateJwtToken(string token)
        {
            try
            {
                var key = Convert.FromBase64String(_config["Jwt:Key"]);
                var symmetricKey = new SymmetricSecurityKey(key);

                var tokenHandler = new JwtSecurityTokenHandler();
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = symmetricKey
                };

                var principal = tokenHandler.ValidateToken(token, validationParameters, out _);
                return principal;
            }
            catch
            {
                return null; // Token inválido o expirado
            }
        }

    }
}
