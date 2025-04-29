using BichoApi.Domain.Entities.User;
using BichoApi.Domain.Interfaces.User;
using BichoApi.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace BichoApi.Infrastructure.Repositories.User;

public class UserRepository(ApiContext context) : IUserRepository
{
    public async Task<IEnumerable<UserEntity>> GetAllUsers()
    {
        return await context.Set<UserEntity>().AsNoTracking().ToListAsync();
    }

    public async Task<UserEntity?> GetUserById(int id)
    {
        return await context.Set<UserEntity>().FindAsync(id);
    }

    public async Task<UserEntity?> GetUserByEmail(string email)
    {
        return await context.Set<UserEntity>().FirstOrDefaultAsync(u => u.Email == email);
    }


    public UserEntity? UpdateUser(UserEntity newUser, int id)
    {
        UserEntity? oldUser = context.Set<UserEntity>().Find(id);
        if (oldUser == null) return null;
        context.Entry(oldUser).CurrentValues.SetValues(newUser);
        context.SaveChanges();
        return newUser;
    }

    public string? DeleteUser(int id)
    {
        UserEntity? oldUser = context.Set<UserEntity>().Find(id);
        if (oldUser == null) return null;
        context.Set<UserEntity>().Remove(oldUser);
        return "User removed!";
    }
}