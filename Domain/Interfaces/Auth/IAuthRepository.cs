using BichoApi.Domain.Entities.Auth;

namespace BichoApi.Domain.Interfaces.Auth;

public interface IAuthRepository
{
    Task<AuthEntity> CreateUser(AuthEntity auth);
    Task<RepositoryClaims?> GetUserByEmail(string email);
}