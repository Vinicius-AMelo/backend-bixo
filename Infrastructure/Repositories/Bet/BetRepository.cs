using BichoApi.Domain.Entities.Bet;
using BichoApi.Domain.Interfaces.Bet;
using BichoApi.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace BichoApi.Infrastructure.Repositories.Bet;

public class BetRepository(ApiContext context) : IBetRepository
{
    public async Task<BetEntity> CreateUser(BetEntity bet)
    {
        await context.Set<BetEntity>().AddAsync(bet);
        await context.SaveChangesAsync();
        return bet;
    }

    public async Task WinningBet(int lotteryId, int betId)
    {
        var winningBet = await context.Set<BetEntity>()
            .FirstOrDefaultAsync(bet => bet.Id == betId && bet.LotteryId == lotteryId);

        if (winningBet != null)
        {
            winningBet.Winning = true;

            await context.SaveChangesAsync();
        }
    }
}