using bixoApi.Context;
using bixoApi.Models.User;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace bixoApi.Repositories.User;

public class UserRepository : IUserRepository
{
    private readonly ApiContext _context;

    public UserRepository(ApiContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<UserEntity>> GetAllUsers()
    {
        return await _context.Set<UserEntity>().AsNoTracking().ToListAsync();
    }

    public async Task<UserEntity?> GetUserById(int id)
    {
        return await _context.Set<UserEntity>().FindAsync(id);
    }
    
    public async Task<UserEntity?> GetUserByEmail(string email)
    {
        return await _context.Set<UserEntity>().FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<UserEntity> CreateUser(UserEntity user)
    {
        await _context.Set<UserEntity>().AddAsync(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public UserEntity? UpdateUser(UserEntity newUser, int id)
    {
        UserEntity oldUser = _context.Set<UserEntity>().Find(id);
        if (oldUser == null) return null;
        _context.Entry(oldUser).CurrentValues.SetValues(newUser);
        _context.SaveChanges();
        return newUser;
    }

    public String? DeleteUser(int id)
    {
        UserEntity oldUser = _context.Set<UserEntity>().Find(id);
        if (oldUser == null) return null;
        _context.Set<UserEntity>().Remove(oldUser);
        return "User removed!";
    }
}