using EarbudExplorer.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Packet_Sniffers.Data;

public class ApplicationDbContext(
    DbContextOptions<ApplicationDbContext> options
) : IdentityDbContext(options)
{
    // Represents the Brands table.
    public DbSet<Brand> Brands { get; set; }

    // Represents the Earbuds table.
    public DbSet<Earbud> Earbuds { get; set; }

    // Represents the Features table.
    public DbSet<Feature> Features { get; set; }

    // Represents the EarbudFeatures junction table.
    public DbSet<EarbudFeature> EarbudFeatures { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Preserve ASP.NET Core Identity's database configuration.
        base.OnModelCreating(modelBuilder);

        // An EarbudFeature record is uniquely identified
        // by the combination of EarbudId and FeatureId.
        modelBuilder.Entity<EarbudFeature>()
            .HasKey(ef => new { ef.EarbudId, ef.FeatureId });

        // An earbud belongs to one brand.
        // A brand can have many earbuds.
        // Prevent deleting a brand while earbuds still reference it.
        modelBuilder.Entity<Earbud>()
            .HasOne(e => e.Brand)
            .WithMany(b => b.Earbuds)
            .HasForeignKey(e => e.BrandId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
