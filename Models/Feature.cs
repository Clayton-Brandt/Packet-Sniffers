
using System.ComponentModel.DataAnnotations;

namespace EarbudExplorer.Models
{
    // Represents an optional capability of an earbud.
    // One feature can be assigned to multiple earbuds.
    public class Feature
    {
        // Primary key for the feature.
        public int FeatureId { get; set; }

        // Feature name is required.
        [Required(ErrorMessage = "Feature name is required.")]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        // Explains what the feature does.
        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        // Navigation property for the many-to-many relationship.
        public ICollection<EarbudFeature> EarbudFeatures { get; set; }
            = new List<EarbudFeature>();
    }
}
