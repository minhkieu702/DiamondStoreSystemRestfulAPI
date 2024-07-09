using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace DiamondStoreSystem.DataLayer.Models
{
    public class Util
    {
        public static string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
    public class DiamondStoreSystemDBContext : DbContext
    {
        public DbSet<Accessory> Accessories { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Diamond> Diamonds { get; set; }
        public DbSet<Diamond> Orders { get; set; }
        public DbSet<SubDiamond> SubDiamonds { get; set; }
        public DbSet<Warranty> Warranties { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<VnPaymentResponse> VnPaymentResponses { get; set; }

        private string GetConnectionString()
        {
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .Build();
            var strConn = config.GetConnectionString("DefaultConnection");
            return strConn;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=tcp:dss.database.windows.net,1433;Initial Catalog=DiamondStoreSytemDB;Persist Security Info=False;User ID=dssswd391;Password=SWD391dss.;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;", sqlOptions => sqlOptions.CommandTimeout(180));
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            // Set up relationships for Order
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Customer)
                .WithMany(a => a.OrdersCustomer)
                .HasForeignKey(o => o.CustomerID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.EmployeeAccount)
                .WithMany(a => a.OrdersStaff)
                .HasForeignKey(o => o.EmployeeAssignID)
                .OnDelete(DeleteBehavior.Restrict);

            // Set up relationships for Product
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Order)
                .WithMany(o => o.Products)
                .HasForeignKey(p => p.OrderID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.MainDiamond)
                .WithOne()
                .HasForeignKey<Product>(p => p.MainDiamondID)
                .OnDelete(DeleteBehavior.Restrict);

            // Set up relationships for SubDiamond
            modelBuilder.Entity<SubDiamond>()
                .HasOne(sd => sd.Product)
                .WithMany(p => p.SubDiamonds)
                .HasForeignKey(sd => sd.ProductID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SubDiamond>()
                .HasOne(sd => sd.Diamond)
                .WithOne()
                .HasForeignKey<SubDiamond>(sd => sd.SubDiamondID)
                .OnDelete(DeleteBehavior.Restrict);

            // Set up relationships for VnPaymentResponse
            modelBuilder.Entity<VnPaymentResponse>()
                .HasOne(p => p.Order)
                .WithOne(o => o.VnPaymentResponse)
                .HasForeignKey<VnPaymentResponse>(p => p.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            // Set up relationships for Warranty
            modelBuilder.Entity<Warranty>()
                .HasOne(w => w.Product)
                .WithOne(p => p.Warranty)
                .HasForeignKey<Warranty>(w => w.ProductID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Account>()
                .HasData(
                new Account
                {
                    AccountID = "S001",
                    Email = "admin@example.com",
                    Password = Util.HashPassword("string"),
                    LastName = "Admin",
                    FirstName = "Super",
                    Phone = "1234567890",
                    Address = "123 Admin St.",
                    Gender = 1,
                    DOB = new DateTime(1980, 1, 1),
                    JoinDate = DateTime.Now,
                    LoyaltyPoint = 0,
                    Block = false,
                    Role = 3,
                    WorkingSchedule = 1
                },
                new Account
                {
                    AccountID = "S002",
                    Email = "staff1@example.com",
                    Password = Util.HashPassword("string"),
                    LastName = "Admin",
                    FirstName = "Super",
                    Phone = "1234567890",
                    Address = "123 Admin St.",
                    Gender = 1,
                    DOB = new DateTime(1980, 1, 1),
                    JoinDate = DateTime.Now,
                    LoyaltyPoint = 0,
                    Block = false,
                    Role = 1,
                    WorkingSchedule = 1
                },
                new Account
                {
                    AccountID = "C001",
                    Email = "customer@example.com",
                    Password = Util.HashPassword("string"),
                    LastName = "Customer",
                    FirstName = "Regular",
                    Phone = "9876543210",
                    Address = "456 Customer Ave.",
                    Gender = 0,
                    DOB = new DateTime(1990, 1, 1),
                    JoinDate = DateTime.Now,
                    LoyaltyPoint = 100,
                    Block = false,
                    Role = 0,
                    WorkingSchedule = 0
                }
                );
            modelBuilder.Entity<Accessory>().HasData(new Accessory
            {
                AccessoryID = "A001",
                AccessoryName = "Gold Chain",
                Description = "18k gold chain",
                Material = 1,
                Style = 1,
                Brand = "LuxuryBrand",
                Block = false,
                Price = 500.0,
                UnitInStock = 10,
                SKU = "GC001",
                ImageUrl = "https://firebasestorage.googleapis.com/v0/b/diamond-store-system.appspot.com/o/365225250_603232441966413_6481642414490509587_n.jpg?alt=media&token=606330be-f47b-45cf-b157-1a9de4f1f91c"
            });
            modelBuilder.Entity<Diamond>().HasData(new Diamond
            {
                DiamondID = "D001",
                Origin = "Africa",
                LabCreated = 0,
                TablePercent = 57.5,
                DepthPercent = 62.0,
                Description = "Brilliant cut diamond",
                GIAReportNumber = 123456,
                IssueDate = new DateTime(2020, 1, 1),
                Shape = 1,
                CaratWeight = 1.0,
                ColorGrade = 1,
                ClarityGrade = 1,
                CutGrade = 1,
                PolishGrade = 1,
                SymmetryGrade = 1,
                FluoresceneGrade = 1,
                Inscription = "GIA12345446",
                Height = 4.0,
                Width = 6.0,
                Length = 6.0,
                Price = 500.0,
                Block = false,
                SKU = "DIA001",
                ImageURL = "https://firebasestorage.googleapis.com/v0/b/diamond-store-system.appspot.com/o/GP1Lty9XgAAI3l-.jpg?alt=media&token=3d8c0b2e-37bd-44ec-a489-9dcc14482312"
            },
            new Diamond
            {
                DiamondID = "D002",
                Origin = "Africa",
                LabCreated = 0,
                TablePercent = 57.5,
                DepthPercent = 62.0,
                Description = "Brilliant cut diamond",
                GIAReportNumber = 123456,
                IssueDate = new DateTime(2020, 1, 1),
                Shape = 1,
                CaratWeight = 1.0,
                ColorGrade = 1,
                ClarityGrade = 1,
                CutGrade = 1,
                PolishGrade = 1,
                SymmetryGrade = 1,
                FluoresceneGrade = 1,
                Inscription = "GIA12345446",
                Height = 4.0,
                Width = 6.0,
                Length = 6.0,
                Price = 500.0,
                Block = false,
                SKU = "DIA001",
                ImageURL = "https://assets.entrepreneur.com/content/3x2/2000/20160305000536-diamond.jpeg"
            });
        }
    }
}
