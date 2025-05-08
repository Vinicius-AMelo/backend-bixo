using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BichoApi.Domain.Entities.Lottery;

[Table("Lottery")]
public class LotteryEntity
{
    [Key] public int Id { get; init; }

    [Required] public required List<int> Draw { get; init; }

    public DateTime CreatedAt { get; init; }
}