
using System.ComponentModel.DataAnnotations;

namespace EarbudExplorer.Models
{
    // Represents a company that manufactures earbuds.
    // One brand can have multiple earbud products.
    public class Brand
    {
        // Primary key that uniquely identifies each brand.
        public int BrandId { get; set; }

        // Requires a brand name with a maximum of 100 characters.
        [Required(ErrorMessage = "Brand name is required.")]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        // Navigation property for the one-to-many relationship.
        // One Brand can be associated with many Earbuds.
        public ICollection<Earbud> Earbuds { get; set; }
            = new List<Earbud>();
    }
}
