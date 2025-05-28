using BichoApi.Domain.Entities.User;

namespace BichoApi.Domain.Interfaces.User;

public interface IUserRepository
{
    Task<IEnumerable<UserEntity>> GetAllUsers();
    Task<UserEntity?> GetUserById(int id);
    Task<UserEntity?> GetUserByEmail(string email);
    UserEntity? UpdateUser(UserEntity user, int id);
    Task<UserEntity?> UpdateUserBalance(int id, int value);
    string? DeleteUser(int id);
}