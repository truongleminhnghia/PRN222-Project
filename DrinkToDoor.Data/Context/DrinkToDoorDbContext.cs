
using DrinkToDoor.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace DrinkToDoor.Data.Context
{
    public class DrinkToDoorDbContext : DbContext
    {
        public DrinkToDoorDbContext() { }

        public DrinkToDoorDbContext(DbContextOptions<DrinkToDoorDbContext> options) : base(options)
        {
        }

        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Kit> Kits { get; set; }
        public DbSet<KitIngredient> KitIngredients { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<KitProduct> KitProducts { get; set; }
        public DbSet<IngredientProduct> IngredientProducts { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<PackagingOption> PackagingOptions { get; set; }
        public DbSet<Message> Messages { get; set; }


        private static readonly TimeZoneInfo _vnZone =
                TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");

        // Trả về DateTime ở múi VN
        private DateTime GetCurrentVnTime()
        {
            return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, _vnZone);
        }

        public override int SaveChanges()
        {
            ApplyTimestamps();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(
            bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default)
        {
            ApplyTimestamps();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void ApplyTimestamps()
        {
            DateTime now = GetCurrentVnTime();

            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = now;
                    entry.Entity.UpdatedAt = now;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Property(x => x.CreatedAt).IsModified = false;
                    entry.Entity.UpdatedAt = now;
                }
            }
        }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Message>()
                .HasOne(m => m.Sender)
                .WithMany(u => u.SentMessages)
                .HasForeignKey(m => m.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Message>()
                .HasOne(m => m.Receiver)
                .WithMany(u => u.ReceivedMessages)
                .HasForeignKey(m => m.ReceiverId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}