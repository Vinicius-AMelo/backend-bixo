using bixoApi.Models.Auth;
using bixoApi.Models.Resultados;
using bixoApi.Models.User;
using Microsoft.EntityFrameworkCore;

namespace bixoApi.Context
{
    public class ApiContext : DbContext
    {

        public ApiContext(DbContextOptions<ApiContext> options) : base(options)
        {
        }

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
}
