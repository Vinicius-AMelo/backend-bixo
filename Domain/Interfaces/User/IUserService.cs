using BichoApi.Domain.Entities.Auth;
using BichoApi.Domain.Entities.User;
using BichoApi.Presentation.DTO.Auth;

namespace BichoApi.Domain.Interfaces.User;

public interface IUserService
{
    public Task<IEnumerable<UserEntity>> GetAllUsers();
    public Task<UserEntity?> GetUserById(int id);
    Task<UserEntity?> GetUserByEmail(string email);

    public Task<UserAuth> CreateUser(AuthDto authDto);
}