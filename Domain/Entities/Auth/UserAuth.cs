using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BichoApi.Domain.Entities.User;

namespace BichoApi.Domain.Entities.Auth;

[Table("Auth")]
public class UserAuth
{
    [Key] [ForeignKey(nameof(UserEntity))] public int UserId { get; init; }

    [Required]
    [StringLength(24, MinimumLength = 8)]
    public required string Password { get; init; }

    public required UserEntity User { get; init; }
}