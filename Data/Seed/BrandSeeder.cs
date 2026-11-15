
using EarbudExplorer.Models;
using Microsoft.EntityFrameworkCore;
using Packet_Sniffers.Data;

namespace Packet_Sniffers.Data.Seed
{
    // Adds the initial earbud brands to our database.
    public static class BrandSeeder
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            // The brands we want available in Earbud Explorer.
            string[] brandNames =
            {
                "Apple",
                "Sony",
                "Bose",
                "Samsung",
                "JBL",
                "Jabra",
                "Sennheiser",
                "Soundcore",
                "Nothing",
                "Technics"
            };

            // Go through each brand name.
            foreach (string brandName in brandNames)
            {
                // Check whether this brand already exists.
                // This prevents duplicates when the seeder runs again.
                bool exists = await context.Brands
                    .AnyAsync(b => b.Name == brandName);

                // Only insert the brand if it doesn't exist.
                if (!exists)
                {
                    Brand brand = new Brand
                    {
                        Name = brandName
                    };

                    context.Brands.Add(brand);
                }
            }

            // Save all newly added brands to SQLite.
            await context.SaveChangesAsync();
        }
    }
}
