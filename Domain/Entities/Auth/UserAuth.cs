using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using bixoApi.Models.User;

namespace bixoApi.Models.Auth;

[Table("Auth")]
public class UserAuth
{
    [Key, ForeignKey(nameof(UserEntity))]
    public int UserId { get; set; }
    
    [Required, StringLength(24, MinimumLength = 8)]
    public required string Password { get; set; }   
    
    public required UserEntity User { get; set; }
}