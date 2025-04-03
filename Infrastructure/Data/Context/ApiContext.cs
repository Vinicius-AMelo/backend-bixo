using BichoApi.Domain.Entities.Auth;
using BichoApi.Domain.Entities.Results;
using BichoApi.Domain.Entities.User;
using Microsoft.EntityFrameworkCore;

namespace BichoApi.Infrastructure.Data.Context;

public class ApiContext(DbContextOptions<ApiContext> options) : DbContext(options)
{
    public DbSet<UserEntity> User { get; set; }
    public DbSet<AuthEntity> UserAuth { get; set; }
    // public DbSet<ResultsEntity> Results { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AuthEntity>()
            .HasOne(ua => ua.User)
            .WithOne()
            .HasForeignKey<AuthEntity>(ua => ua.UserId)
            .IsRequired();

        base.OnModelCreating(modelBuilder);
    }
}