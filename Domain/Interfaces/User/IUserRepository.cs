using BichoApi.Domain.Entities.Auth;
using BichoApi.Domain.Entities.User;
using BichoApi.Presentation.DTO.Auth;
using Microsoft.AspNetCore.Mvc;

namespace BichoApi.Domain.Interfaces.User;

public interface IUserRepository
{
    Task<IEnumerable<UserEntity>> GetAllUsers();
    Task<UserEntity?> GetUserById(int id);
    Task<UserEntity?> GetUserByEmail(string email);
    Task<UserAuth> CreateUser(UserAuth userAuth);
    UserEntity? UpdateUser(UserEntity user, int id);
    string? DeleteUser(int id);
}