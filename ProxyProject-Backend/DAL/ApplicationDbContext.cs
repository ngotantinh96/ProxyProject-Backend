using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using ProxyProject_Backend.DAL.Entities;
using System.Reflection.Emit;

namespace ProxyProject_Backend.DAL
{
    public class ApplicationDbContext : IdentityDbContext<UserEntity>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Relationship config
            builder.Entity<ProxyKeyPlansEntity>()
                .HasMany(x => x.ProxyKeys)
                .WithOne(x => x.ProxyKeyPlan)
                .HasForeignKey(x => x.ProxyKeyPlanId);

            builder.Entity<UserEntity>()
                .HasMany(x => x.ProxyKeys)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId);

            builder.Entity<UserEntity>()
               .HasMany(x => x.WalletHistories)
               .WithOne(x => x.User)
               .HasForeignKey(x => x.UserId);

            // Seed data config
            builder.Entity<ProxyKeyPlansEntity>().HasData(new ProxyKeyPlansEntity 
            { 
                Id = new Guid("2dfa909c-3cd6-494e-9e99-5267b64eb791"),
                Name = "Key Vip",
                Price = 16000,
                PriceUnit = "đ/key/ngày",
                Description = "Được quyền đổi IP sau: 2 phút, IP sống đến khi người dùng đổi IP (IP private), tốc độ vượt trội"
            });
        }

        public DbSet<ProxyKeyPlansEntity> ProxyKeyPlans { get; set; }
        public DbSet<ProxyKeysEntity> ProxyKeys { get; set; }
        public DbSet<WalletHistoryEntity> WalletHistory { get; set; }
        public DbSet<BankAccountEntity> BankAccounts { get; set; }
        public DbSet<NotificationEntity> Notifications { get; set; }
    }
}
