using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using QRWithSignalR.Entity;

namespace QRWithSignalR.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
         : base(options)
        {
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<PurchaseItems> PurchaseItems { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // 🔑 Apply global precision for all decimal properties
            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                var decimalProps = entityType.GetProperties()
                    .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?));

                foreach (var property in decimalProps)
                {
                    property.SetPrecision(18); // total digits
                    property.SetScale(4);      // digits after decimal point
                }
            }

            // Example: if you want specific configuration for PurchaseItems.Price
            builder.Entity<PurchaseItems>()
                .Property(p => p.Price)
                   .HasPrecision(18, 4);
        }
    }
}
