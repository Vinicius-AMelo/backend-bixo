using BichoApi.Domain.Entities.Auth;
using BichoApi.Domain.Entities.User;
using BichoApi.Domain.Interfaces.User;
using BichoApi.Presentation.DTO.Auth;

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

    public async Task<UserAuth> CreateUser(AuthDto authDto)
    {
        var userAuth = new UserAuth
        {
            Password = authDto.Password,
            User = new UserEntity
            {
                Email = authDto.Email,
                Name = authDto.Email
            }

        };
        
        return await userRepository.CreateUser(userAuth);
    }
}