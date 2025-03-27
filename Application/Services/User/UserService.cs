using bixoApi.Models.User;
using bixoApi.Repositories.User;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace bixoApi.Services.User;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<UserEntity>> GetAllUsers()
    {
        return await _userRepository.GetAllUsers();
    }

    public async Task<UserEntity?> GetUserById(int id)
    {
        return await _userRepository.GetUserById(id);
    }
    
    public async Task<UserEntity?> GetUserByEmail(string email)
    {
        return await _userRepository.GetUserByEmail(email);
    }

    public async Task<UserEntity> CreateUser(UserEntity user)
    {
        return await _userRepository.CreateUser(user);
    }
}