using BichoApi.Application.Hubs;
using BichoApi.Domain.Entities.Bet;
using Microsoft.AspNetCore.SignalR;

namespace BichoApi.Application.Services.Bet;

public class BetService : BackgroundService
{
    private readonly IHubContext<GameHub> _hubContext;
    private readonly Random _random = new();

    public BetService(IHubContext<GameHub> hubContext)
    {
        _hubContext = hubContext;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            List<int> drawnNumbers = GenerateNumbers();

            await _hubContext.Clients.All.SendAsync("Sorteio", drawnNumbers);

            List<BetEntity> winners = GameHub.CurrentBets
                .Where(bet => bet.Bet.Any(n => drawnNumbers.Contains(n)))
                .ToList();

            foreach (BetEntity winner in winners)
                await _hubContext.Clients.Client(winner.ConnectionId)
                    .SendAsync("Vencedor", drawnNumbers);

            GameHub.CurrentBets.Clear();

            await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
        }
    }

    private List<int> GenerateNumbers()
    {
        // return new List<int> { 1, 2, 3, 4, 5 };
        return Enumerable.Range(0, 99)
            .OrderBy(_ => _random.Next())
            .Take(5)
            .ToList();
    }
}