using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace BichoApi.Infrastructure.JwtHelper
{
    public static class JwtHelper
    {
        public static string GerarToken(TokenClaims user, string secretKey)
        {
            var key = Encoding.UTF8.GetBytes(secretKey);

            var claims = new[]
            {
                new Claim("id", user.Id),
                new Claim("email", user.Email),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim("balance", user.Balance)
            };

            SecurityTokenDescriptor tokenDescriptor = new()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            JwtSecurityTokenHandler tokenHandler = new();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public static TokenClaims? ValidarToken(string token)
        {
            var key = Encoding.UTF8.GetBytes("chave_muito_grande_e_segura_de_32+_caracteres");
            JwtSecurityTokenHandler tokenHandler = new();

            try
            {
                var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ClockSkew = TimeSpan.Zero
                }, out var validatedToken);

                var id = principal.FindFirst("id")?.Value;
                var email = principal.FindFirst("email")?.Value;
                var role = principal.FindFirst(ClaimTypes.Role)?.Value;
                var balance = principal.FindFirst("balance")?.Value;

                return new TokenClaims(id!, email!, role!, balance!);
            }
            catch
            {
                return null;
            }
        }
    }
}

public record TokenClaims(string Id, string Email, string Role, string Balance);

public record RepositoryClaims(int Id, string Email, string Role, string Password, int Balance);