using InventoryAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryAPI.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Product>? Products { get; set; }
        public DbSet<Category>? Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) 
        {
            #region Category
            modelBuilder.Entity<Category>().HasKey(c => c.CategoryId);
            modelBuilder.Entity<Category>().Property(c => c.Name)
                                           .HasMaxLength(100)
                                           .IsRequired();
            modelBuilder.Entity<Category>().Property(c => c.Description)
                                           .HasMaxLength(175)
                                           .IsRequired();
            #endregion

            #region Product
            modelBuilder.Entity<Product>().HasKey(c => c.ProductId);
            modelBuilder.Entity<Product>().Property(c => c.Name)
                                           .HasMaxLength(100)
                                           .IsRequired();
            modelBuilder.Entity<Product>().Property(c => c.Description)
                                           .HasMaxLength(175)
                                           .IsRequired();
            modelBuilder.Entity<Product>().Property(c => c.Image)
                                          .HasMaxLength(100)
                                          .IsRequired();
            modelBuilder.Entity<Product>().Property(c => c.Price).HasPrecision(14, 2);
            #endregion

            #region Relationship
            modelBuilder.Entity<Product>().HasOne<Category>(c => c.Category)
                                          .WithMany(p => p.Products)
                                          .HasForeignKey(c => c.CategoryId);
            #endregion
        }
    }
}
