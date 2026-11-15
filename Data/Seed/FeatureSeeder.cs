
using EarbudExplorer.Models;
using Microsoft.EntityFrameworkCore;
using Packet_Sniffers.Data;

namespace Packet_Sniffers.Data.Seed
{
    // This class adds our initial earbud features to the database.
    public static class FeatureSeeder
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            // Each feature has a name and a description.
            // These describe capabilities that earbuds may support.
            var features = new[]
            {
                new { Name = "Wireless Charging",
                      Description = "Supports charging with a compatible wireless charging pad." },

                new { Name = "Multipoint Bluetooth",
                      Description = "Can connect to multiple devices at the same time." },

                new { Name = "Spatial Audio",
                      Description = "Provides a spatial listening experience." },

                new { Name = "Wear Detection",
                      Description = "Detects when earbuds are worn or removed." },

                new { Name = "Customizable Controls",
                      Description = "Allows users to customize earbud controls." },

                new { Name = "Fast Charging",
                      Description = "Provides additional listening time after a short charge." },

                new { Name = "Companion App",
                      Description = "Supports settings through a companion mobile app." },

                new { Name = "Earbud Location Tracking",
                      Description = "Helps users locate misplaced earbuds." },

                new { Name = "Voice Assistant Integration",
                      Description = "Supports interaction with a voice assistant." },

                new { Name = "Low-Latency Gaming Mode",
                      Description = "Reduces audio delay during gaming." }
            };

            // Go through each feature in our list.
            foreach (var featureData in features)
            {
                // Check whether the feature already exists.
                // This prevents duplicate records when restarting the app.
                bool exists = await context.Features
                    .AnyAsync(f => f.Name == featureData.Name);

                // Only add features that are missing.
                if (!exists)
                {
                    Feature feature = new Feature
                    {
                        Name = featureData.Name,
                        Description = featureData.Description
                    };

                    context.Features.Add(feature);
                }
            }

            // Save all new features to the SQLite database.
            await context.SaveChangesAsync();
        }
    }
}
