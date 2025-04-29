using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BichoApi.Domain.Entities.User;

namespace BichoApi.Domain.Entities.Auth;

[Table("Auth")]
public class AuthEntity
{
    [Key] [ForeignKey(nameof(UserEntity))] public int UserId { get; init; }

    [Required] [StringLength(100)] public required string Password { get; init; }

    public required UserEntity User { get; init; }
}