using bixoApi.Models.User;
using Microsoft.AspNetCore.Mvc;

namespace bixoApi.Repositories.User;

public interface IUserRepository
{
    Task<IEnumerable<UserEntity>> GetAllUsers();
    Task<UserEntity?> GetUserById(int id);
    Task<UserEntity?> GetUserByEmail(string email);
    Task<UserEntity> CreateUser(UserEntity user);
    UserEntity? UpdateUser(UserEntity user, int id);
    String? DeleteUser(int id);
    
}