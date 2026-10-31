
namespace EarbudExplorer.Models
{
    // Junction table connecting earbuds and features.
    // This allows one earbud to have multiple features
    // and one feature to belong to multiple earbuds.
    public class EarbudFeature
    {
        // Foreign key referencing the Earbud table.
        // Also forms part of the composite primary key.
        public int EarbudId { get; set; }

        // Navigation property for the related earbud.
        public Earbud Earbud { get; set; } = null!;

        // Foreign key referencing the Feature table.
        // Also forms part of the composite primary key.
        public int FeatureId { get; set; }

        // Navigation property for the related feature.
        public Feature Feature { get; set; } = null!;
    }
}
