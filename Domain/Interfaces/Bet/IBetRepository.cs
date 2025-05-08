using BichoApi.Domain.Entities.Bet;

namespace BichoApi.Domain.Interfaces.Bet;

public interface IBetRepository
{
    Task<BetEntity> CreateUser(BetEntity bet);
    Task WinningBet(int lotteryId, int betId);
}