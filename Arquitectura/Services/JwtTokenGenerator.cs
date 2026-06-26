using Aplicacion.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Arquitectura.Services
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly IConfiguration _configuration;

        public JwtTokenGenerator(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public string Generate(int usuarioId, string nombre, string rol)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var credenciales = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, usuarioId.ToString()),
                new(JwtRegisteredClaimNames.UniqueName, nombre),
                new(ClaimTypes.Role, rol),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var minutos = int.TryParse(_configuration["Jwt:DurationInMinutes"], out var m) ? m : 60;

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(minutos),
                signingCredentials: credenciales);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
