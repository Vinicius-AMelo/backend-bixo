using BichoApi.Domain.Entities.Auth;
using BichoApi.Domain.Entities.User;
using BichoApi.Domain.Interfaces.Auth;
using BichoApi.Infrastructure.JwtHelper;
using BichoApi.Presentation.DTO.Auth;

namespace BichoApi.Application.Services.Auth;

public class AuthService(IAuthRepository authRepository, IConfiguration config) : IAuthService
{
    public async Task<UserEntity> CreateUser(RegisterDto registerDto)
    {
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);
        UserEntity user = new() { Email = registerDto.Email, Name = registerDto.Name, Role = "user" };
        AuthEntity auth = new() { Password = passwordHash, User = user };

        await authRepository.CreateUser(auth);
        return user;
    }

    public async Task<string?> GetUserByEmail(LoginDto loginDto)
    {
        var secretKey = config["JWTKey"];
        var user = await authRepository.GetUserByEmail(loginDto.Email);
        if (user == null) return null;
        var matchPassword = BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password);
        if (!matchPassword) return null;
        var token = JwtHelper.GerarToken(
            new TokenClaims(user.Id.ToString(), user.Email, user.Role, user.Balance.ToString()),
            secretKey
        );
        return token;
    }
}