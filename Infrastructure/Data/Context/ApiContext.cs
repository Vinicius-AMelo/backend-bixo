using BichoApi.Domain.Entities.Auth;
using BichoApi.Domain.Entities.Results;
using BichoApi.Domain.Entities.User;
using Microsoft.EntityFrameworkCore;

namespace BichoApi.Infrastructure.Data.Context;

public class ApiContext(DbContextOptions<ApiContext> options) : DbContext(options)
{
    public DbSet<UserEntity> User { get; set; }
    public DbSet<UserAuth> UserAuth { get; set; }
    public DbSet<ResultsEntity> Results { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserEntity>()
            .HasOne(u => u.UserAuth)
            .WithOne(a => a.User)
            .HasForeignKey<UserAuth>(a => a.UserId)
            .IsRequired();

        base.OnModelCreating(modelBuilder);
    }
}