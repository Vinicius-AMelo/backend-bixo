using System.ComponentModel.DataAnnotations;
using BichoApi.Domain.Entities.Lottery;
using BichoApi.Domain.Entities.User;

namespace BichoApi.Domain.Entities.Bet;

public class BetEntity
{
    [Key] public int Id { get; init; }

    public string ConnectionId { get; init; } = "";
    [Required] public required List<int> Bet { get; init; }

    [Required] public required int LotteryId { get; init; }

    [Required] public required LotteryEntity Lottery { get; init; }

    [Required] public required int UserId { get; init; }

    [Required] public required UserEntity User { get; init; }

    [Required] public required bool? Winning { get; set; }

    public DateTime CreatedAt { get; init; }
}