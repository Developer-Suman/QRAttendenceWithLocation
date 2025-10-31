using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
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

            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    if (property.ClrType == typeof(bool))
                    {
                        property.SetValueConverter(new BoolToZeroOneConverter<int>());
                        property.SetColumnType("NUMBER(1)");
                    }
                    else if (property.ClrType == typeof(bool?))
                    {
                        property.SetValueConverter(new BoolToZeroOneConverter<int?>());
                        property.SetColumnType("NUMBER(1)");
                    }
                }
            }

            builder.Entity<PurchaseItems>(entity =>
            {
                entity.ToTable("PURCHASEITEMS"); // Uppercase, no quotes

                entity.HasKey(e => e.Id);

                entity.Property(e => e.ItemName)
                      .HasColumnName("ITEMNAME")  // Uppercase
                      .HasColumnType("NVARCHAR2(100)")
                      .IsRequired();

                entity.Property(e => e.Price)
                      .HasColumnName("PRICE")
                      .HasColumnType("NUMBER");

                entity.Property(e => e.Quantity)
                      .HasColumnName("QUANTITY")
                      .HasColumnType("NUMBER");

                entity.Property(e => e.QRUrl)
                      .HasColumnName("QRURL")
                      .HasColumnType("NVARCHAR2(200)");
            });


            builder.Entity<ApplicationUser>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.FirstName)
                      .HasColumnType("NVARCHAR2(2000)");

                entity.Property(e => e.LastName)
                      .HasColumnType("NVARCHAR2(2000)");

                entity.Property(e => e.Address)
                      .HasColumnType("NVARCHAR2(2000)");

                entity.Property(e => e.EmailConfirmed)
                      .HasColumnType("NUMBER(1)");


                entity.Property(e => e.PhoneNumberConfirmed)
                      .HasColumnType("NUMBER(1)");


                entity.Property(e => e.TwoFactorEnabled)
                      .HasColumnType("NUMBER(1)");


                entity.Property(e => e.LockoutEnabled)
                      .HasColumnType("NUMBER(1)");


                entity.Property(e => e.AccessFailedCount)
                      .HasColumnType("NUMBER(10)");
                  

                entity.Property(e => e.LockoutEnd)
                      .HasColumnType("TIMESTAMP(7) WITH TIME ZONE");
            });





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
