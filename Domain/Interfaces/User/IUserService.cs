using BichoApi.Domain.Entities.User;

namespace BichoApi.Domain.Interfaces.User;

public interface IUserService
{
    public Task<IEnumerable<UserEntity>> GetAllUsers();
    public Task<UserEntity?> GetUserById(int id);
}