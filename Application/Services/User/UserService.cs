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
}