using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using FourLeafCloverShoe.Share.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using FourLeafCloverShoe.Share.Models;

namespace FourLeafCloverShoe.Data
{
    public class MyDbContext : IdentityDbContext<User>
    {
        public MyDbContext(DbContextOptions<MyDbContext> options)
          : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {

            base.OnModelCreating(builder);
            Create(builder);
        }
        private void Create(ModelBuilder builder)
        {
            builder.Entity<User>().ToTable("Users", "security");
            builder.Entity<IdentityRole>().ToTable("Roles", "security");
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles", "security");
            builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims", "security");
            builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins", "security");
            builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims", "security");
            builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens", "security");
            builder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = "2FA6148D-B530-421F-878E-CE1D54BFC6AB", Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole { Id = "2FA6148D-B530-421F-878E-CE2D54BFC6AB", Name = "User", NormalizedName = "USER" },
                new IdentityRole { Id = "2FA6148D-B530-421F-878E-CE4D54BFC6AB", Name = "Staff", NormalizedName = "STAFF" }
            );
            builder.Entity<User>().HasData(
                    new User() { Id = "2FA6148D-B530-421F-878E-CE4D54BFC6AB", Coins = 0, Points = 0, UserName = "Guest", AccessFailedCount = 0, RankId = Guid.Parse("2FA0118D-B530-421F-878E-CE4D54BFC6AB"), LockoutEnabled = true, TwoFactorEnabled = false, PhoneNumberConfirmed = false, EmailConfirmed = false, NormalizedUserName = "GUEST" },
                    new User() { Id = "1FA6148D-B530-421F-878E-CE4D54BFC6AB", Coins = 0, Points = 0, Email = "admin@gmail.com", UserName = "Admin", PasswordHash = "AQAAAAEAACcQAAAAEBU/ECKQGqvUa243/dkXqtMpJ0yhaEGc9ZnA0+MgtG0aWOrjfJhk6L0/xQc4fuAbtg==", AccessFailedCount = 0, RankId = Guid.Parse("2FA0118D-B530-421F-878E-CE4D54BFC6AB"), LockoutEnabled = true, TwoFactorEnabled = false, PhoneNumberConfirmed = false, EmailConfirmed = true, NormalizedUserName = "ADMIN" } //Adminmeo123@
                );
            builder.Entity<IdentityUserRole<string>>().HasData(
                    new IdentityUserRole<string>() { UserId = "2FA6148D-B530-421F-878E-CE4D54BFC6AB", RoleId = "2FA6148D-B530-421F-878E-CE4D54BFC6AB" },
                    new IdentityUserRole<string>() { UserId = "1FA6148D-B530-421F-878E-CE4D54BFC6AB", RoleId = "2FA6148D-B530-421F-878E-CE1D54BFC6AB" }
            );
            builder.Entity<Ranks>().HasData(
                new Ranks() { Id = Guid.Parse("2FA0118D-B530-421F-878E-CE4D54BFC6AB"), Name = "Bạc", PointsMin = 0, PoinsMax = 1000000 },
                new Ranks() { Id = Guid.NewGuid(), Name = "Vàng", PointsMin = 1000001, PoinsMax = 3000000 },
                new Ranks() { Id = Guid.NewGuid(), Name = "Kim Cương", PointsMin = 3000001, PoinsMax = 10000000 }
            );

            builder.Entity<Brand>().HasData(
                new Brand() { Id = Guid.NewGuid(), Name = "Nike" },
                new Brand() { Id = Guid.NewGuid(), Name = "Adidas" },
                new Brand() { Id = Guid.NewGuid(), Name = "Biti's" },
                new Brand() { Id = Guid.NewGuid(), Name = "Ananas" },
                new Brand() { Id = Guid.NewGuid(), Name = "Vascara" },
                new Brand() { Id = Guid.NewGuid(), Name = "Juno" },
                new Brand() { Id = Guid.NewGuid(), Name = "Thượng Đình" },
                new Brand() { Id = Guid.NewGuid(), Name = "Laforce" },
                new Brand() { Id = Guid.NewGuid(), Name = "MWC" },
                new Brand() { Id = Guid.NewGuid(), Name = "Đông Hải" }
            );

            builder.Entity<Share.Models.Size>().HasData(
                new Size() { Id = Guid.NewGuid(), Name = "38" },
                new Size() { Id = Guid.NewGuid(), Name = "39" },
                new Size() { Id = Guid.NewGuid(), Name = "40" },
                new Size() { Id = Guid.NewGuid(), Name = "41" },
                new Size() { Id = Guid.NewGuid(), Name = "42" },
                new Size() { Id = Guid.NewGuid(), Name = "43" },
                new Size() { Id = Guid.NewGuid(), Name = "44" }
            );

            builder.Entity<Category>().HasData(
            new Category() { Id = Guid.NewGuid(), Name = "Sneakers" },
            new Category() { Id = Guid.NewGuid(), Name = "Giày cao gót" },
            new Category() { Id = Guid.NewGuid(), Name = "Giày sandal" },
            new Category() { Id = Guid.NewGuid(), Name = "Dép" },
            new Category() { Id = Guid.NewGuid(), Name = "Giày thể thao" },
            new Category() { Id = Guid.NewGuid(), Name = "Giày lười" },
            new Category() { Id = Guid.NewGuid(), Name = "Giày boot" },
            new Category() { Id = Guid.NewGuid(), Name = "Giày da" }
        );

            builder.Entity<PaymentType>().HasData(
                new PaymentType() { Id = Guid.NewGuid(), Name = "cod", Status = true },
                new PaymentType() { Id = Guid.NewGuid(), Name = "momo", Status = true },
                new PaymentType() { Id = Guid.NewGuid(), Name = "vnpay", Status = true }
                );
            builder.Entity<Colors>().HasData(
                new Colors() { Id = Guid.NewGuid(), ColorName = "Đỏ", ColorCode = "#FF0000" },
                new Colors() { Id = Guid.NewGuid(), ColorName = "Cam", ColorCode = "#FFA500" },
                new Colors() { Id = Guid.NewGuid(), ColorName = "Vàng", ColorCode = "#FFFF00" },
                new Colors() { Id = Guid.NewGuid(), ColorName = "Lục", ColorCode = "#008000" },
                new Colors() { Id = Guid.NewGuid(), ColorName = "Lam", ColorCode = "#0000FF" },
                new Colors() { Id = Guid.NewGuid(), ColorName = "Chàm", ColorCode = "#4B0082" },
                new Colors() { Id = Guid.NewGuid(), ColorName = "Tím", ColorCode = "#800080" },
                new Colors() { Id = Guid.NewGuid(), ColorName = "Hồng", ColorCode = "#FFC0CB" },
                new Colors() { Id = Guid.NewGuid(), ColorName = "Nâu", ColorCode = "#A52A2A" },
                new Colors() { Id = Guid.NewGuid(), ColorName = "Xám", ColorCode = "#808080" },
                new Colors() { Id = Guid.NewGuid(), ColorName = "Đen", ColorCode = "#000000" },
                new Colors() { Id = Guid.NewGuid(), ColorName = "Trắng", ColorCode = "#FFFFFF" },
                new Colors() { Id = Guid.NewGuid(), ColorName = "Ngọc lam", ColorCode = "#40E0D0" },
                new Colors() { Id = Guid.NewGuid(), ColorName = "Hồng đào", ColorCode = "#FFDAB9" },
                new Colors() { Id = Guid.NewGuid(), ColorName = "Tím hoa cà", ColorCode = "#E6E6FA" }
            );
        }
        public DbSet<Colors> Colors { get; set; }
        public DbSet<PaymentDetail> PaymentDetails { get; set; }
        public DbSet<PaymentType> PaymentTypes { get; set; }
        public DbSet<Address> Address { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductDetail> ProductDetails { get; set; }
        public DbSet<ProductImages> ProductImages { get; set; }
        public DbSet<Ranks> Ranks { get; set; }
        public DbSet<Rate> Rates { get; set; }
        public DbSet<Size> Sizes { get; set; }
        public DbSet<Material> materials { get; set; }
        public DbSet<Voucher> Vouchers { get; set; }
        public DbSet<UserVoucher> UserVouchers { get; set; }

    }
}
