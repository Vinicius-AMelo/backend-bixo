namespace BichoApi.Domain.Entities.Bet;

public class BetEntity
{
    public string ConnectionId { get; set; } = string.Empty;
    public List<int> Bet { get; set; } = new();
}