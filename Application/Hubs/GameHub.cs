using BichoApi.Domain.Entities.Bet;
using BichoApi.Domain.Interfaces.Bet;
using BichoApi.Domain.Interfaces.ILotteryRepository;
using BichoApi.Domain.Interfaces.User;
using Microsoft.AspNetCore.SignalR;

namespace BichoApi.Application.Hubs;

public class GameHub(IBetRepository betRepository, IUserService userRepository, ILotteryRepository lotteryRepository)
    : Hub
{
    public static List<BetEntity> CurrentBets { get; set; } = new();
    public static Dictionary<int, Queue<BetEntity>> UserLastBets { get; set; } = new();

    public async Task CreateBet(List<int> bet, int userId, int value)
    {
        try
        {
            // CurrentBets.RemoveAll(a => a.UserId == userId);
            var user = await userRepository.GetUserById(userId);
            var lottery = await lotteryRepository.LastLottery();

            if (user == null)
            {
                await Clients.Caller.SendAsync("UserNotFound");
                return;
            }

            if (user.Balance < value)
            {
                await Clients.Caller.SendAsync("SaldoInsuficiente");
                return;
            }

            var newBet = new BetEntity
            {
                ConnectionId = Context.ConnectionId,
                User = user,
                UserId = user.Id,
                Bet = bet,
                Lottery = lottery,
                LotteryId = lottery.Id,
                Winning = false,
                Value = value
            };

            if (!UserLastBets.ContainsKey(user.Id))
                UserLastBets[user.Id] = new Queue<BetEntity>();

            var queue = UserLastBets[user.Id];
            queue.Enqueue(newBet);
            if (queue.Count > 4)
                queue.Dequeue();

            CurrentBets.Add(newBet);
            await userRepository.UpdateUserBalance(userId, value * -1);
            await betRepository.CreateUser(newBet);

            await Clients.Caller.SendAsync("ApostaConfirmada", bet);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task JoinSession(string sessionId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, sessionId);
    }

    public async Task LeaveSession(string sessionId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, sessionId);
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        CurrentBets.RemoveAll(a => a.ConnectionId == Context.ConnectionId);
        return base.OnDisconnectedAsync(exception);
    }
}