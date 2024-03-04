// Import necessary namespaces for Entity Framework Core and your model classes.
using Microsoft.EntityFrameworkCore;
using WebAPIDemo.Models;

namespace WebAPIDemo.Data
{
    // Define your custom application DbContext.
    public class ApplicationDbContext : DbContext
    {
        // Constructor to initialize the context with options.
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
            // Constructor body.
        }

        // DbSet property to represent the collection of Shirt entities in the database.
        public DbSet<Shirt> Shirts { get; set; }

        // Override OnModelCreating method to configure the model.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Call base implementation of OnModelCreating.
            base.OnModelCreating(modelBuilder);

            // Configure data seeding for the Shirt entities.
            modelBuilder.Entity<Shirt>().HasData(
                new Shirt { ShirtId = 1, Brand = "My Brand", Color = "Blue", Gender = "Men", Price = 30, Size = 10 },
                new Shirt { ShirtId = 2, Brand = "My Brand1", Color = "Black", Gender = "Men", Price = 35, Size = 12 },
                new Shirt { ShirtId = 3, Brand = "My Brand2", Color = "Pink", Gender = "Women", Price = 20, Size = 8 },
                new Shirt { ShirtId = 4, Brand = "My Brand3", Color = "Yellow", Gender = "Women", Price = 28, Size = 9 }
            );
        }
    }
}
