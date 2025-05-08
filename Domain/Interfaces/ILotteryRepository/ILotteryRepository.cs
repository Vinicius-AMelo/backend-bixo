using BichoApi.Domain.Entities.Lottery;

namespace BichoApi.Domain.Interfaces.ILotteryRepository;

public interface ILotteryRepository
{
    Task<LotteryEntity> CreateLottery(LotteryEntity lottery);
    Task<LotteryEntity> LastLottery();
}