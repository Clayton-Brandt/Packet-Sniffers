using System.ComponentModel.DataAnnotations;

namespace EarbudExplorer.Models
{
    // Represents the information a client sends
    // when creating or updating an earbud.
    public class EarbudRequest
    {
        [Required]
        [StringLength(150)]
        public string Name { get; set; } = string.Empty;

        [Range(0, 99999999.99)]
        public decimal Price { get; set; }

        [Range(0, 200)]
        public int BatteryLife { get; set; }

        [Range(0, 5)]
        public decimal Rating { get; set; }

        public bool NoiseCancellation { get; set; }

        public bool TransparencyMode { get; set; }

        [StringLength(20)]
        public string WaterResistance { get; set; } = string.Empty;

        [StringLength(20)]
        public string BluetoothVersion { get; set; } = string.Empty;

        [Range(1, int.MaxValue)]
        public int BrandId { get; set; }

        // IDs of existing features to associate with the earbud.
        public List<int> FeatureIds { get; set; } = new();
    }
}