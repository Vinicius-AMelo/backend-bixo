using BichoApi.Domain.Entities.User;

namespace BichoApi.Presentation.DTO.Auth;

public class AuthDto
{
    public required string Email { get; set; }
    public required string Name { get; set; }
    public required string Password { get; set; }
}