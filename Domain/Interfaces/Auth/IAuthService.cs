using BichoApi.Domain.Entities.User;
using BichoApi.Presentation.DTO.Auth;

namespace BichoApi.Domain.Interfaces.Auth;

public interface IAuthService
{
    Task<UserEntity> CreateUser(RegisterDto registerDto);
    Task<string?> GetUserByEmail(LoginDto loginDto);
}