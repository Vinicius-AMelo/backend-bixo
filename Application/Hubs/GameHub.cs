using BichoApi.Domain.Entities.Bet;
using Microsoft.AspNetCore.SignalR;

namespace BichoApi.Application.Hubs;

public class GameHub : Hub
{
    public static List<BetEntity> CurrentBets { get; set; } = new();

    public async Task CreateBet(List<int> bet)
    {
        CurrentBets.RemoveAll(a => a.ConnectionId == Context.ConnectionId);

        CurrentBets.Add(new BetEntity
        {
            ConnectionId = Context.ConnectionId,
            Bet = bet
        });

        await Clients.Caller.SendAsync("ApostaConfirmada", bet);
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        CurrentBets.RemoveAll(a => a.ConnectionId == Context.ConnectionId);
        return base.OnDisconnectedAsync(exception);
    }
}