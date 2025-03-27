using bixoApi.Models.User;
using Microsoft.AspNetCore.Mvc;

namespace bixoApi.Services.User;

public interface IUserService
{
    public Task<IEnumerable<UserEntity>> GetAllUsers();
    public Task<UserEntity?> GetUserById(int id);
    Task<UserEntity?> GetUserByEmail(string email);

    public Task<UserEntity> CreateUser(UserEntity user);
}