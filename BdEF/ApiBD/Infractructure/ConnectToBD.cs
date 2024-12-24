using ApiBD.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiBD.Infractructure
{
    public class ConnectToBD : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }

        /// <summary>
        /// Настройка подключения 
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=Shop;Username=postgres;Password=1HUF!zLRnCKM-kV0");
        }

        /// <summary>
        /// Fluent API для настройки маппинга моделей с таблицами, тут указано:
        /// 1. Настройки для ключей 
        /// 2. Связь между "Users" и "Orders"
        /// 3. Связь между "Orders" и "OrderDetails"
        /// 4. Связь между "Products" и "OrderDetails"
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(u => new { u.Id });
            modelBuilder.Entity<Order>().HasKey(u => new { u.OrderID });
            modelBuilder.Entity<Product>().HasKey(u => new { u.ProductID });
            modelBuilder.Entity<OrderDetails>().HasKey(u => new { u.OrderDetailsID });

            modelBuilder.Entity<User>().HasMany(o => o.OrdersList).WithOne(u => u.User).HasForeignKey(k => k.UserID);
            modelBuilder.Entity<Order>().HasMany(o => o.orderDetailsList).WithOne(u => u.Order).HasForeignKey(k => k.OrderID);
            modelBuilder.Entity<Product>().HasMany(o => o.orderDetailsList).WithOne(u => u.Product).HasForeignKey(k => k.ProductID);
        }

    }
}
