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
            byte[] key = Encoding.UTF8.GetBytes(secretKey);

            Claim[] claims = new[]
            {
                new Claim("id", user.Id),
                new Claim("email", user.Email),
                new Claim(ClaimTypes.Role, user.Role)
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
            SecurityToken? token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public static TokenClaims? ValidarToken(string token)
        {
            byte[] key = Encoding.UTF8.GetBytes("chave_muito_grande_e_segura_de_32+_caracteres");
            JwtSecurityTokenHandler tokenHandler = new();

            try
            {
                ClaimsPrincipal? principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                string? id = principal.FindFirst("id")?.Value;
                string? email = principal.FindFirst("email")?.Value;
                string? role = principal.FindFirst(ClaimTypes.Role)?.Value;

                return new TokenClaims(id!, email!, role!);
            }
            catch
            {
                return null;
            }
        }
    }
}

public record TokenClaims(string Id, string Email, string Role);

public record RepositoryClaims(int Id, string Email, string Role, string Password);