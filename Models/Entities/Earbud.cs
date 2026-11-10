
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace EarbudExplorer.Models
{
    // Represents an earbud product that users can search,
    // filter, view, and compare in the application.
    public class Earbud
    {
        // Primary key for the earbud.
        public int EarbudId { get; set; }

        // Product name is required.
        [Required(ErrorMessage = "Earbud name is required.")]
        [StringLength(150)]
        public string Name { get; set; } = string.Empty;

        // Price must be zero or greater.
        // Precision defines the total digits and decimal places.
        [Precision(10, 2)]
        [Range(0, 99999999.99)]
        public decimal Price { get; set; }

        // Battery life is measured in hours.
        // The range prevents negative or unreasonable values.
        [Range(0, 200)]
        public int BatteryLife { get; set; }

        // Rating must be between 0 and 5.
        [Precision(3, 2)]
        [Range(0, 5)]
        public decimal Rating { get; set; }

        // Indicates whether active noise cancellation is supported.
        public bool NoiseCancellation { get; set; }

        // Indicates whether transparency mode is supported.
        public bool TransparencyMode { get; set; }

        // Water resistance classification, such as IPX4.
        [StringLength(20)]
        public string WaterResistance { get; set; } = string.Empty;

        // Bluetooth version, such as 5.3.
        [StringLength(20)]
        public string BluetoothVersion { get; set; } = string.Empty;

        // Foreign key connecting this earbud to its manufacturer.
        [Range(1, int.MaxValue, ErrorMessage = "Please select a brand.")]
        public int BrandId { get; set; }

        // Navigation property for the associated brand.
        public Brand Brand { get; set; } = null!;

        // Navigation property connecting earbuds to features
        // through the EarbudFeature junction table.
        public ICollection<EarbudFeature> EarbudFeatures { get; set; }
            = new List<EarbudFeature>();
    }
}
