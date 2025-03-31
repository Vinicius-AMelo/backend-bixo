using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BichoApi.Domain.Entities.Auth;

namespace BichoApi.Domain.Entities.User;

[Table("Users")]
public class UserEntity
{
    [Key] public int Id { get; init; }

    [Required] [StringLength(50)] public required string Name { get; init; }

    [Required] [StringLength(50)] public required string Email { get; init; }
}