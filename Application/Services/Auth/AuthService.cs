using BichoApi.Domain.Entities.Auth;
using BichoApi.Domain.Entities.User;
using BichoApi.Domain.Interfaces.Auth;
using BichoApi.Presentation.DTO.Auth;

namespace BichoApi.Application.Services.Auth;

public class AuthService(IAuthRepository authRepository) : IAuthService
{
    public async Task<UserEntity> CreateUser(RegisterDto registerDto)
    {
        var user = new UserEntity { Email = registerDto.Email, Name = registerDto.Name };
        var auth = new AuthEntity { Password = registerDto.Password, User = user };

        await authRepository.CreateUser(auth);
        return user;
    }

    public async Task<string?> GetUserByEmail(LoginDto loginDto)
    {
        var password = await authRepository.GetUserByEmail(loginDto.Email);
        if (password == null) return null;
        if (password != loginDto.Password) return null;
        return password;
    }
}