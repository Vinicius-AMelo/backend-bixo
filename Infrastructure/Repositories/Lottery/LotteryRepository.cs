using BichoApi.Domain.Entities.Lottery;
using BichoApi.Domain.Interfaces.ILotteryRepository;
using BichoApi.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace BichoApi.Infrastructure.Repositories.Lottery;

public class LotteryRepository(ApiContext context) : ILotteryRepository
{
    public async Task<LotteryEntity> CreateLottery(LotteryEntity lottery)
    {
        await context.Set<LotteryEntity>().AddAsync(lottery);
        await context.SaveChangesAsync();
        return lottery;
    }

    public async Task<LotteryEntity> LastLottery()
    {
        var lastLottery = await context.Set<LotteryEntity>().OrderByDescending(lt => lt.CreatedAt)
            .FirstOrDefaultAsync();

        return lastLottery;
    }
}