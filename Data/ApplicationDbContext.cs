
using EarbudExplorer.Models;
using Microsoft.EntityFrameworkCore;

namespace EarbudExplorer.Data
{
    // Represents the connection between our C# models
    // and the SQL Server database.
    //
    // Entity Framework Core uses this class to create,
    // read, update, and delete database records.
    public class ApplicationDbContext : DbContext
    {
        // Constructor receives database configuration
        // from the dependency injection system in Program.cs.
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Each DbSet represents a table in the database.

        // Stores all earbud manufacturers.
        public DbSet<Brand> Brands { get; set; }

        // Stores all earbud products.
        public DbSet<Earbud> Earbuds { get; set; }

        // Stores all available earbud features.
        public DbSet<Feature> Features { get; set; }

        // Junction table connecting earbuds and features.
        public DbSet<EarbudFeature> EarbudFeatures { get; set; }

        // Configures database relationships and keys
        // when Entity Framework creates the database.

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Apply the default Entity Framework Core configurations.
            base.OnModelCreating(modelBuilder);

            // Configure the relationship between Brand and Earbud.
            // One brand can have many earbuds, but each earbud
            // belongs to one brand.
            modelBuilder.Entity<Earbud>()
                .HasOne(e => e.Brand)
                .WithMany(b => b.Earbuds)
                .HasForeignKey(e => e.BrandId)
                .OnDelete(DeleteBehavior.NoAction);

            // Configure the many-to-many relationship
            // between earbuds and features.
            //
            // The combination of EarbudId and FeatureId
            // must be unique in the junction table.
            modelBuilder.Entity<EarbudFeature>()
                .HasKey(ef => new { ef.EarbudId, ef.FeatureId });
        }

    }
}
