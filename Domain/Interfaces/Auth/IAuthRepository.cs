using BichoApi.Domain.Entities.Auth;
using BichoApi.Presentation.DTO.Auth;

namespace BichoApi.Domain.Interfaces.Auth;

public interface IAuthRepository
{
    Task<AuthEntity> CreateUser(AuthEntity auth);
    Task<string?> GetUserByEmail(string email);
}