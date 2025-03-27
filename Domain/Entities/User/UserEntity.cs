using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using bixoApi.Models.Auth;

namespace bixoApi.Models.User
{
    [Table("Users")]
    public class UserEntity
    {
        [Key]
        public int Id { get; set; }
        
        [Required, StringLength(50)]
        public required string Name { get; set; }
        
        [Required, StringLength(50)]
        public required string Email { get; set; }
        
        public required UserAuth UserAuth { get; set; }
    }
}
