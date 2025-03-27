using BichoApi.Domain.Entities.User;
using BichoApi.Domain.Interfaces.User;

namespace BichoApi.Application.Services.User;

public class UserService(IUserRepository userRepository) : IUserService
{
    public async Task<IEnumerable<UserEntity>> GetAllUsers()
    {
        return await userRepository.GetAllUsers();
    }

    public async Task<UserEntity?> GetUserById(int id)
    {
        return await userRepository.GetUserById(id);
    }

    public async Task<UserEntity?> GetUserByEmail(string email)
    {
        return await userRepository.GetUserByEmail(email);
    }

    public async Task<UserEntity> CreateUser(UserEntity user)
    {
        return await userRepository.CreateUser(user);
    }
}