using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProxyProject_Backend.DAL.Entities;
using ProxyProject_Backend.Utils;
using static ProxyProject_Backend.Controllers.BankAccountController;

namespace ProxyProject_Backend.DAL
{
    public class ApplicationDbContext : IdentityDbContext<UserEntity>
    {
        private readonly IConfiguration _configuration;
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options,
            IConfiguration configuration
            ) : base(options)
        {
            _configuration = configuration;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Relationship config
            builder.Entity<ProxyKeyPlansEntity>()
                .HasMany(x => x.ProxyKeys)
                .WithOne(x => x.ProxyKeyPlan)
                .HasForeignKey(x => x.ProxyKeyPlanId);

            builder.Entity<ProxyKeyPlansEntity>()
              .HasMany(x => x.Proxies)
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

            builder.Entity<UserEntity>()
               .HasMany(x => x.TransactionHistories)
               .WithOne(x => x.User)
               .HasForeignKey(x => x.UserId);

            builder.Entity<UserEntity>()
               .HasMany(x => x.ProxyHistories)
               .WithOne(x => x.User)
               .HasForeignKey(x => x.UserId);

            // Seed data config
            builder.Entity<IdentityRole>().HasData(new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id = "848e7b9c-5781-48cb-aa51-c2ee49961828",
                    Name = UserRolesConstant.User,
                    NormalizedName = UserRolesConstant.User.ToUpper()
                },
                new IdentityRole
                {
                    Id = "fb2ec114-eb3c-499d-ab9d-ab8cb579c329",
                    Name = UserRolesConstant.Admin,
                    NormalizedName = UserRolesConstant.Admin.ToUpper()
                }
            });


            var hasher = new PasswordHasher<UserEntity>();

            var adminAccounts = _configuration.GetSection("AdminAccounts").Get<List<AdminAccount>>();

            builder.Entity<UserEntity>().HasData(adminAccounts.Select(x => new UserEntity
            {
                Id = x.Id,
                Email = x.Email,
                NormalizedEmail = x.Email.ToUpper(),
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = x.UserName,
                NormalizedUserName = x.UserName.ToUpper(),
                APIKey = StringUtils.GenerateSecureKey(),
                WalletKey = StringUtils.GenerateSecureKey(),
                TwoFactorEnabled = true,
                LimitKeysToCreate = x.LimitKeysToCreate,
                PasswordHash = hasher.HashPassword(null, x.Password),
            }));

            builder.Entity<IdentityUserRole<string>>().HasData(adminAccounts.Select(x => new IdentityUserRole<string>
            {
                UserId = x.Id,
                RoleId = "fb2ec114-eb3c-499d-ab9d-ab8cb579c329"
            }));

            builder.Entity<ProxyKeyPlansEntity>().HasData(new ProxyKeyPlansEntity
            {
                Id = new Guid("2dfa909c-3cd6-494e-9e99-5267b64eb791"),
                Name = "Key Vip",
                Code = "VN",
                Price = 16000,
                PriceUnit = "đ/key/ngày",
                Description = "Được quyền đổi IP sau: 2 phút, IP sống đến khi người dùng đổi IP (IP private), tốc độ vượt trội"
            });
        }

        public DbSet<ProxyKeyPlansEntity> ProxyKeyPlans { get; set; }
        public DbSet<ProxyKeysEntity> ProxyKeys { get; set; }
        public DbSet<ProxyEntity> Proxy { get; set; }
        public DbSet<WalletHistoryEntity> WalletHistory { get; set; }
        public DbSet<BankAccountEntity> BankAccounts { get; set; }
        public DbSet<NotificationEntity> Notifications { get; set; }
        public DbSet<TransactionHistoryEntity> TransactionHistories { get; set; }
        public DbSet<ProxyHistoryEntity> ProxyHistory { get; set; }
    }
}
