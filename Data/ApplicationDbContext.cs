
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
            // Applies the default Entity Framework configurations.
            base.OnModelCreating(modelBuilder);

            // Defines a composite primary key for EarbudFeature.
            //
            // Both EarbudId and FeatureId together identify
            // a unique relationship between an earbud and a feature.
            //
            // This prevents duplicate earbud-feature combinations.
            modelBuilder.Entity<EarbudFeature>()
                .HasKey(ef => new { ef.EarbudId, ef.FeatureId });
        }
    }
}
