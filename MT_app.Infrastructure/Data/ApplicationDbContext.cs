using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MT_app.core.Models;
using MT_app.Infrastructure.Data.AuthenModels;

namespace MT_app.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // builder.Entity<IdentityRole>().HasData(
            //     new IdentityRole(){ Name = "Admin", ConcurrencyStamp = "1", NormalizedName = "Admin"},
            //     new IdentityRole(){ Name = "Manager", ConcurrencyStamp = "2", NormalizedName = "Manager"},
            //     new IdentityRole(){ Name = "Staff", ConcurrencyStamp = "3", NormalizedName = "Staff" }
            // );

            builder.Entity<Customer>()
                .HasIndex(c => c.PhoneNumber)
                .IsUnique();

            builder.ApplyConfiguration(new AppEntityConfiguration());
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entities = ChangeTracker.Entries()
                .Where(x => x.Entity is BaseEntity &&
                            (x.State == EntityState.Added || x.State == EntityState.Modified));

            foreach (var entity in entities)
            {
                var entityObject = (BaseEntity)entity.Entity;
                if (entity.State == EntityState.Added)
                {
                    entityObject.CreatedDate = DateTime.UtcNow;
                }

                entityObject.UpdatedDate = DateTime.UtcNow;
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}