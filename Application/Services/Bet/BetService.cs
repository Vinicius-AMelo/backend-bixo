using BichoApi.Application.Hubs;
using BichoApi.Domain.Entities.Bet;
using BichoApi.Domain.Entities.Lottery;
using BichoApi.Domain.Interfaces.Bet;
using BichoApi.Domain.Interfaces.ILotteryRepository;
using BichoApi.Domain.Interfaces.User;
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
                var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();


                var drawnNumbers = GenerateNumbers();
                await lotteryRepository.CreateLottery(new LotteryEntity { Draw = drawnNumbers });

                await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);

                await _hubContext.Clients.All.SendAsync("Sorteio", drawnNumbers, stoppingToken);

                var winners = GameHub.CurrentBets
                    .Where(bet =>
                        bet.Bet.Any(n => drawnNumbers.Contains(n)) ||
                        (GameHub.UserLastBets.TryGetValue(bet.UserId, out var queue) && IsDoublingBetSequence(queue))
                    )
                    .ToList();

                foreach (var winner in winners)
                {
                    await betRepository.WinningBet(winner.LotteryId, winner.Id);
                    await userRepository.UpdateUserBalance(winner.UserId, winner.Value * 2);
                    await _hubContext.Clients.Client(winner.ConnectionId)
                        .SendAsync("Vencedor", drawnNumbers, stoppingToken);
                }

                var sessionResults = GameHub.CurrentBets.Select(bet => new
                {
                    User = bet.User.Name,
                    bet.Bet,
                    IsWinner = drawnNumbers.Any(n => bet.Bet.Contains(n)),
                    bet.Value
                }).ToList();

                await _hubContext.Clients.All
                    .SendAsync("ResultadosSessao", sessionResults, drawnNumbers, stoppingToken);
            }

            GameHub.CurrentBets.Clear();
        }
    }

    private List<int> GenerateNumbers()
    {
        return Enumerable.Range(1, 25)
            .OrderBy(_ => _random.Next())
            .Take(4)
            .ToList();
    }

    private bool IsDoublingBetSequence(IEnumerable<BetEntity> last4Bets)
    {
        var orderedBets = last4Bets.OrderBy(b => b.CreatedAt).ToList();
        if (orderedBets.Count < 4)
            return false;

        if (orderedBets.Select(b => b.LotteryId).Distinct().Count() < 4)
            return false;

        for (var i = 1; i < orderedBets.Count; i++)
            if (orderedBets[i].Value != orderedBets[i - 1].Value * 2)
                return false;

        // Console.WriteLine("dobrinha");
        return true;
    }
}