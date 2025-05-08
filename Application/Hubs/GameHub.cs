using BichoApi.Domain.Entities.Bet;
using BichoApi.Domain.Interfaces.Bet;
using BichoApi.Domain.Interfaces.ILotteryRepository;
using BichoApi.Domain.Interfaces.User;
using Microsoft.AspNetCore.SignalR;

namespace BichoApi.Application.Hubs;

public class GameHub(IBetRepository betRepository, IUserRepository userRepository, ILotteryRepository lotteryRepository)
    : Hub
{
    public static List<BetEntity> CurrentBets { get; set; } = new();

    public async Task CreateBet(List<int> bet, int userId)
    {
        try
        {
            CurrentBets.RemoveAll(a => a.UserId == userId);
            var user = await userRepository.GetUserById(userId);
            var lottery = await lotteryRepository.LastLottery();

            var newBet = new BetEntity
            {
                ConnectionId = Context.ConnectionId,
                User = user,
                UserId = user.Id,
                Bet = bet,
                Lottery = lottery,
                LotteryId = lottery.Id,
                Winning = false
            };

            CurrentBets.Add(newBet);

            await betRepository.CreateUser(newBet);

            await Clients.Caller.SendAsync("ApostaConfirmada", bet);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        CurrentBets.RemoveAll(a => a.ConnectionId == Context.ConnectionId);
        return base.OnDisconnectedAsync(exception);
    }
}