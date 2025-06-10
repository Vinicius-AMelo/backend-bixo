using BichoApi.Domain.Entities.Auth;
using BichoApi.Domain.Interfaces.Auth;
using BichoApi.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace BichoApi.Infrastructure.Repositories.Auth;

public class AuthRepository(ApiContext context) : IAuthRepository
{
    public async Task<AuthEntity> CreateUser(AuthEntity auth)
    {
        await context.Set<AuthEntity>().AddAsync(auth);
        await context.SaveChangesAsync();
        return auth;
    }

    public async Task<RepositoryClaims?> GetUserByEmail(string email)
    {
        var loginData = await context.Set<AuthEntity>()
            .Include(a => a.User)
            .Select(a => new { a.User.Id, a.User.Email, a.Password, a.User.Role, a.User.Balance })
            .FirstOrDefaultAsync(u => u.Email == email);
        if (loginData == null) return null;
        return new RepositoryClaims(loginData.Id, loginData.Email, loginData.Role, loginData.Password,
            loginData.Balance);
    }
}