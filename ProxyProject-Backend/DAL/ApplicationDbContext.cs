using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProxyProject_Backend.DAL.Entities;
using ProxyProject_Backend.Utils;

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
            builder.Entity<UserEntity>().HasData(new UserEntity
            {
                Id = "03a35a7f-e8f9-4856-adb3-f7e548dce6b7",
                Email = _configuration["AdminAccount:Email"],
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = _configuration["AdminAccount:UserName"],
                APIKey = StringUtils.GenerateSecureKey(),
                WalletKey = StringUtils.GenerateSecureKey(),
                TwoFactorEnabled = true,
                LimitKeysToCreate = int.Parse(_configuration["AdminAccount:LimitKeysToCreate"]),
                PasswordHash = hasher.HashPassword(null, _configuration["AdminAccount:Password"]),

            });

            builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                UserId = "03a35a7f-e8f9-4856-adb3-f7e548dce6b7",
                RoleId = "fb2ec114-eb3c-499d-ab9d-ab8cb579c329"
            });

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


    }
}
