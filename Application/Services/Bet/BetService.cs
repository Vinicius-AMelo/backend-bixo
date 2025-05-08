using BichoApi.Application.Hubs;
using BichoApi.Domain.Entities.Lottery;
using BichoApi.Domain.Interfaces.Bet;
using BichoApi.Domain.Interfaces.ILotteryRepository;
using Microsoft.AspNetCore.SignalR;

namespace BichoApi.Application.Services.Bet;

public class BetService : BackgroundService
{
    private readonly IHubContext<GameHub> _hubContext;
    private readonly Random _random = new();
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public BetService(IHubContext<GameHub> hubContext, IServiceScopeFactory serviceScopeFactory)
    {
        _hubContext = hubContext;
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var lotteryRepository = scope.ServiceProvider.GetRequiredService<ILotteryRepository>();
                var betRepository = scope.ServiceProvider.GetRequiredService<IBetRepository>();


                var drawnNumbers = GenerateNumbers();
                lotteryRepository.CreateLottery(new LotteryEntity { Draw = drawnNumbers });

                await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);

                await _hubContext.Clients.All.SendAsync("Sorteio", drawnNumbers);

                var winners = GameHub.CurrentBets
                    .Where(bet => bet.Bet.Any(n => drawnNumbers.Contains(n)))
                    .ToList();

                foreach (var winner in winners)
                {
                    await betRepository.WinningBet(winner.LotteryId, winner.Id);
                    await _hubContext.Clients.Client(winner.ConnectionId)
                        .SendAsync("Vencedor", drawnNumbers);
                }
            }

            GameHub.CurrentBets.Clear();
        }
    }

    private List<int> GenerateNumbers()
    {
        return Enumerable.Range(0, 9)
            .OrderBy(_ => _random.Next())
            .Take(6)
            .ToList();
    }
}