using BichoApi.Domain.Entities.Auth;
using BichoApi.Domain.Entities.Bet;
using BichoApi.Domain.Entities.Lottery;
using BichoApi.Domain.Entities.User;
using Microsoft.EntityFrameworkCore;

namespace BichoApi.Infrastructure.Data.Context;

public class ApiContext(DbContextOptions<ApiContext> options) : DbContext(options)
{
    public DbSet<UserEntity> User { get; set; }

    public DbSet<AuthEntity> UserAuth { get; set; }

    public DbSet<LotteryEntity> Lottery { get; set; }

    public DbSet<BetEntity> Bet { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AuthEntity>()
            .HasOne(ua => ua.User)
            .WithOne()
            .HasForeignKey<AuthEntity>(ua => ua.UserId)
            .IsRequired();

        modelBuilder.Entity<BetEntity>()
            .HasOne(bet => bet.Lottery)
            .WithMany()
            .HasForeignKey(bet => bet.LotteryId)
            .IsRequired();

        modelBuilder.Entity<LotteryEntity>()
            .Property(a => a.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .ValueGeneratedOnAdd();


        base.OnModelCreating(modelBuilder);
    }
}