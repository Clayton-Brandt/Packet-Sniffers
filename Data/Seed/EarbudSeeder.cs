
using EarbudExplorer.Models;
using Microsoft.EntityFrameworkCore;
using Packet_Sniffers.Data;

namespace Packet_Sniffers.Data.Seed
{
    // Adds real earbud products to our catalog.
    public static class EarbudSeeder
    {

        public static async Task SeedAsync(ApplicationDbContext context)
        {
            // Find the Sony brand in our database.
            // We use its name because BrandId can change after a reset.
            Brand? sony = await context.Brands
                .FirstOrDefaultAsync(b => b.Name == "Sony");

            // Stop if Sony hasn't been added to the database.
            if (sony == null)
            {
                throw new Exception("Sony brand was not found.");
            }

            // Add the Sony WF-1000XM5 using our reusable method.
            await AddEarbudAsync(
                context,

                // Product information.
                new Earbud
                {
                    Name = "Sony WF-1000XM5",
                    Price = 329.99m,
                    BatteryLife = 8,
                    Rating = 0,
                    NoiseCancellation = true,
                    TransparencyMode = true,
                    WaterResistance = "IPX4",
                    BluetoothVersion = "5.3",
                    BrandId = sony.BrandId
                },

                // Features supported by this product.
                new string[]
                {
            "Wireless Charging",
            "Multipoint Bluetooth",
            "Fast Charging",
            "Companion App",
            "Customizable Controls"
                }
            );


            // Find Apple using its name, not a fixed BrandId.
            Brand? apple = await context.Brands
                .FirstOrDefaultAsync(b => b.Name == "Apple");

            // Stop if the Apple brand is missing.
            if (apple == null)
            {
                throw new Exception("Apple brand was not found.");
            }

            // Add AirPods Pro 2 with its specifications and features.
            await AddEarbudAsync(
                context,
                new Earbud
                {
                    Name = "Apple AirPods Pro 2 (USB-C)",
                    Price = 249.00m,
                    BatteryLife = 6,
                    Rating = 0,
                    NoiseCancellation = true,
                    TransparencyMode = true,
                    WaterResistance = "IP54",
                    BluetoothVersion = "5.3",
                    BrandId = apple.BrandId
                },
                new string[]
                {
            "Wireless Charging",
            "Spatial Audio",
            "Wear Detection",
            "Fast Charging",
            "Earbud Location Tracking",
            "Voice Assistant Integration"
                }
            );

            // Seed the remaining 48 catalog products.
            await SeedRemainingEarbudsAsync(context);

        }



        // Adds an earbud and connects it to its supported features.
        // This method can be reused for all 50 products.
        private static async Task AddEarbudAsync(
            ApplicationDbContext context,
            Earbud earbud,
            string[] featureNames)
        {
            // Check whether this product already exists.
            Earbud? existingEarbud = await context.Earbuds
                .FirstOrDefaultAsync(e => e.Name == earbud.Name);

            // Insert only when the product is missing.
            // This preserves changes made by administrators.
            if (existingEarbud == null)
            {
                context.Earbuds.Add(earbud);
                await context.SaveChangesAsync();

                existingEarbud = earbud;
            }

            // Connect the earbud to its verified features.
            foreach (string featureName in featureNames)
            {
                // Find the feature by name, not a hard-coded ID.
                Feature? feature = await context.Features
                    .FirstOrDefaultAsync(f => f.Name == featureName);

                if (feature == null)
                {
                    throw new Exception(
                        $"Feature not found: {featureName}");
                }

                // Check whether this relationship already exists.
                bool relationshipExists = await context.EarbudFeatures
                    .AnyAsync(ef =>
                        ef.EarbudId == existingEarbud.EarbudId &&
                        ef.FeatureId == feature.FeatureId);

                // Add only missing relationships.
                if (!relationshipExists)
                {
                    context.EarbudFeatures.Add(new EarbudFeature
                    {
                        EarbudId = existingEarbud.EarbudId,
                        FeatureId = feature.FeatureId
                    });
                }
            }

            // Save the new feature relationships.
            await context.SaveChangesAsync();
        }


        private static async Task SeedRemainingEarbudsAsync(
            ApplicationDbContext context)
        {
            // Each entry contains:
            // Brand, Name, Price, BatteryLife, ANC, Transparency,
            // WaterResistance, BluetoothVersion, FeatureNames.
            //
            // IMPORTANT: These are provisional catalog values.
            // Verify individual specifications before final submission.
            // BatteryLife is intended as single-charge hours, not case total.

            var products = new[]
            {
        // APPLE — 4 more
        ("Apple", "Apple AirPods 2", 129m, 5, false, false, "None", "5.0", "Wear Detection|Voice Assistant Integration"),
        ("Apple", "Apple AirPods 3", 169m, 6, false, false, "IPX4", "5.0", "Spatial Audio|Wear Detection|Fast Charging|Voice Assistant Integration"),
        ("Apple", "Apple AirPods 4", 129m, 5, false, false, "IP54", "5.3", "Spatial Audio|Wear Detection|Fast Charging|Voice Assistant Integration"),
        ("Apple", "Apple AirPods 4 with ANC", 179m, 4, true, true, "IP54", "5.3", "Wireless Charging|Spatial Audio|Wear Detection|Fast Charging|Voice Assistant Integration"),

        // SONY — 4 more
        ("Sony", "Sony WF-1000XM4", 279.99m, 8, true, true, "IPX4", "5.2", "Wireless Charging|Fast Charging|Companion App|Customizable Controls"),
        ("Sony", "Sony WF-C500", 99.99m, 10, false, false, "IPX4", "5.0", "Fast Charging|Companion App"),
        ("Sony", "Sony WF-C700N", 119.99m, 7, true, true, "IPX4", "5.2", "Fast Charging|Companion App|Customizable Controls"),
        ("Sony", "Sony LinkBuds S", 199.99m, 6, true, true, "IPX4", "5.2", "Multipoint Bluetooth|Fast Charging|Companion App"),

        // BOSE — 5 products
        ("Bose", "Bose QuietComfort Ultra Earbuds (1st Gen)", 299m, 6, true, true, "IPX4", "5.3", "Multipoint Bluetooth|Spatial Audio|Wear Detection|Customizable Controls|Fast Charging|Companion App"),
        ("Bose", "Bose QuietComfort Ultra Earbuds (2nd Gen)", 299m, 6, true, true, "IPX4", "5.3", "Wireless Charging|Multipoint Bluetooth|Spatial Audio|Wear Detection|Companion App"),
        ("Bose", "Bose QuietComfort Earbuds II", 299m, 6, true, true, "IPX4", "5.3", "Wear Detection|Fast Charging|Companion App"),
        ("Bose", "Bose QuietComfort Earbuds (2024)", 179m, 8, true, true, "IPX4", "5.3", "Wireless Charging|Multipoint Bluetooth|Customizable Controls|Companion App"),
        ("Bose", "Bose Sport Earbuds", 179m, 5, false, false, "IPX4", "5.1", "Fast Charging|Companion App"),

        // SAMSUNG — 5 products
        ("Samsung", "Samsung Galaxy Buds2", 149.99m, 5, true, true, "IPX2", "5.2", "Wireless Charging|Wear Detection|Companion App"),
        ("Samsung", "Samsung Galaxy Buds2 Pro", 229.99m, 5, true, true, "IPX7", "5.3", "Wireless Charging|Spatial Audio|Wear Detection|Companion App"),
        ("Samsung", "Samsung Galaxy Buds FE", 99.99m, 6, true, true, "IPX2", "5.2", "Wear Detection|Companion App"),
        ("Samsung", "Samsung Galaxy Buds3", 179.99m, 5, false, false, "IP57", "5.4", "Wireless Charging|Spatial Audio|Companion App"),
        ("Samsung", "Samsung Galaxy Buds3 Pro", 249.99m, 6, true, true, "IP57", "5.4", "Wireless Charging|Spatial Audio|Wear Detection|Companion App"),

        // JBL — 5 products
        ("JBL", "JBL Live Pro 2", 149.95m, 8, true, true, "IPX5", "5.2", "Wireless Charging|Multipoint Bluetooth|Companion App"),
        ("JBL", "JBL Tour Pro 2", 249.95m, 8, true, true, "IPX5", "5.3", "Wireless Charging|Multipoint Bluetooth|Companion App"),
        ("JBL", "JBL Tour Pro 3", 299.95m, 8, true, true, "IP55", "5.3", "Wireless Charging|Multipoint Bluetooth|Spatial Audio|Companion App"),
        ("JBL", "JBL Tune Buds", 99.95m, 10, true, true, "IP54", "5.3", "Companion App|Customizable Controls"),
        ("JBL", "JBL Live Beam 3", 199.95m, 10, true, true, "IP55", "5.3", "Wireless Charging|Multipoint Bluetooth|Companion App"),

        // JABRA — 5 products
        ("Jabra", "Jabra Elite 4", 99.99m, 5, true, true, "IP55", "5.2", "Multipoint Bluetooth|Fast Charging|Companion App"),
        ("Jabra", "Jabra Elite 5", 149.99m, 7, true, true, "IP55", "5.2", "Multipoint Bluetooth|Wireless Charging|Companion App"),
        ("Jabra", "Jabra Elite 7 Pro", 199.99m, 8, true, true, "IP57", "5.2", "Wireless Charging|Multipoint Bluetooth|Companion App"),
        ("Jabra", "Jabra Elite 8 Active", 199.99m, 8, true, true, "IP68", "5.3", "Wireless Charging|Multipoint Bluetooth|Companion App"),
        ("Jabra", "Jabra Elite 10 Gen 2", 279.99m, 6, true, true, "IP57", "5.3", "Wireless Charging|Multipoint Bluetooth|Spatial Audio|Companion App"),

        // SENNHEISER — 5 products
        ("Sennheiser", "Sennheiser MOMENTUM True Wireless 3", 249.95m, 7, true, true, "IPX4", "5.2", "Wireless Charging|Customizable Controls|Companion App"),
        ("Sennheiser", "Sennheiser MOMENTUM True Wireless 4", 299.95m, 7, true, true, "IP54", "5.4", "Wireless Charging|Multipoint Bluetooth|Customizable Controls|Companion App"),
        ("Sennheiser", "Sennheiser CX True Wireless", 129.95m, 9, false, false, "IPX4", "5.2", "Customizable Controls|Companion App"),
        ("Sennheiser", "Sennheiser CX Plus True Wireless", 179.95m, 8, true, true, "IPX4", "5.2", "Customizable Controls|Companion App"),
        ("Sennheiser", "Sennheiser ACCENTUM True Wireless", 199.95m, 8, true, true, "IP54", "5.3", "Wireless Charging|Multipoint Bluetooth|Companion App"),

        // SOUNDCORE — 5 products
        ("Soundcore", "Soundcore Liberty 3 Pro", 169.99m, 6, true, true, "IPX4", "5.2", "Wireless Charging|Multipoint Bluetooth|Companion App"),
        ("Soundcore", "Soundcore Liberty 4", 149.99m, 7, true, true, "IPX4", "5.3", "Wireless Charging|Multipoint Bluetooth|Spatial Audio|Wear Detection|Companion App"),
        ("Soundcore", "Soundcore Liberty 4 NC", 99.99m, 8, true, true, "IPX4", "5.3", "Wireless Charging|Multipoint Bluetooth|Companion App"),
        ("Soundcore", "Soundcore Liberty 4 Pro", 129.99m, 7, true, true, "IPX5", "5.3", "Wireless Charging|Multipoint Bluetooth|Companion App"),
        ("Soundcore", "Soundcore Space A40", 99.99m, 8, true, true, "IPX4", "5.2", "Wireless Charging|Multipoint Bluetooth|Companion App"),

        // NOTHING — 5 products
        ("Nothing", "Nothing Ear (1)", 99m, 4, true, true, "IPX4", "5.2", "Wireless Charging|Wear Detection|Companion App"),
        ("Nothing", "Nothing Ear (2)", 149m, 4, true, true, "IP54", "5.3", "Wireless Charging|Multipoint Bluetooth|Wear Detection|Companion App"),
        ("Nothing", "Nothing Ear (2024)", 149m, 5, true, true, "IP54", "5.3", "Wireless Charging|Multipoint Bluetooth|Wear Detection|Companion App"),
        ("Nothing", "Nothing Ear (a)", 99m, 5, true, true, "IP54", "5.3", "Multipoint Bluetooth|Wear Detection|Companion App"),
        ("Nothing", "Nothing Ear (open)", 149m, 8, false, false, "IP54", "5.3", "Multipoint Bluetooth|Companion App"),

        // TECHNICS — 5 products
        ("Technics", "Technics EAH-AZ40", 129.99m, 7, false, true, "IPX4", "5.2", "Multipoint Bluetooth|Companion App"),
        ("Technics", "Technics EAH-AZ40M2", 149.99m, 5, true, true, "IPX4", "5.3", "Multipoint Bluetooth|Companion App"),
        ("Technics", "Technics EAH-AZ60", 229.99m, 7, true, true, "IPX4", "5.2", "Multipoint Bluetooth|Companion App"),
        ("Technics", "Technics EAH-AZ60M2", 249.99m, 7, true, true, "IPX4", "5.3", "Wireless Charging|Multipoint Bluetooth|Companion App"),
        ("Technics", "Technics EAH-AZ80", 299.99m, 7, true, true, "IPX4", "5.3", "Wireless Charging|Multipoint Bluetooth|Wear Detection|Companion App")
    };

            foreach (var product in products)
            {
                // Find the brand that owns this product.
                Brand? brand = await context.Brands
                    .FirstOrDefaultAsync(b => b.Name == product.Item1);

                if (brand == null)
                {
                    throw new Exception(
                        $"Brand not found: {product.Item1}");
                }

                // Create the earbud from its catalog data.
                Earbud earbud = new Earbud
                {
                    Name = product.Item2,
                    Price = product.Item3,
                    BatteryLife = product.Item4,
                    NoiseCancellation = product.Item5,
                    TransparencyMode = product.Item6,
                    WaterResistance = product.Item7,
                    BluetoothVersion = product.Item8,
                    Rating = 0,
                    BrandId = brand.BrandId
                };

                // Convert the pipe-separated feature names into an array.
                string[] featureNames = product.Item9.Split('|');

                // Reuse the method we already built and tested.
                await AddEarbudAsync(context, earbud, featureNames);
            }
        }


    }
}
