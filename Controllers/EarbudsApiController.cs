using EarbudExplorer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Packet_Sniffers.Data;
using Microsoft.AspNetCore.Authorization;

namespace Packet_Sniffers.Controllers
{
    // Handles HTTP requests for earbud information.
    [ApiController]
    [Route("api/earbuds")]
    public class EarbudsApiController : ControllerBase
    {
        // Provides access to the application's SQLite database.
        private readonly ApplicationDbContext _context;

        // Receives the database context through dependency injection.
        public EarbudsApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /api/earbuds
        // Returns all earbuds with their brand and features.
        [HttpGet]
        public async Task<IActionResult> GetEarbuds()
        {
            var earbuds = await _context.Earbuds
                .AsNoTracking()
                .Select(e => new
                {
                    e.EarbudId,
                    e.Name,
                    e.Price,
                    e.BatteryLife,
                    e.Rating,
                    e.NoiseCancellation,
                    e.TransparencyMode,
                    e.WaterResistance,
                    e.BluetoothVersion,
                    e.BrandId,
                    BrandName = e.Brand.Name,

                    Features = e.EarbudFeatures.Select(ef => new
                    {
                        ef.FeatureId,
                        ef.Feature.Name,
                        ef.Feature.Description
                    }).ToList()
                })
                .ToListAsync();

            return Ok(earbuds);
        }

        // GET: /api/earbuds/1
        // Retrieves a specific earbud by its ID.
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEarbud(int id)
        {
            // Find the earbud and select the information
            // that should be returned to the client.
            var earbud = await _context.Earbuds
                .AsNoTracking()
                .Where(e => e.EarbudId == id)
                .Select(e => new
                {
                    e.EarbudId,
                    e.Name,
                    e.Price,
                    e.BatteryLife,
                    e.Rating,
                    e.NoiseCancellation,
                    e.TransparencyMode,
                    e.WaterResistance,
                    e.BluetoothVersion,
                    e.BrandId,
                    BrandName = e.Brand.Name,

                    // Include the features associated with this earbud.
                    Features = e.EarbudFeatures.Select(ef => new
                    {
                        ef.FeatureId,
                        ef.Feature.Name,
                        ef.Feature.Description
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            // Return HTTP 404 if the earbud doesn't exist.
            if (earbud == null)
            {
                return NotFound();
            }

            // Return HTTP 200 with the earbud as JSON.
            return Ok(earbud);
        }

        // POST: /api/earbuds
        // Creates a new earbud and connects it to its brand and features.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateEarbud(EarbudRequest request)
        {
            // Make sure the selected brand exists.
            bool brandExists = await _context.Brands
                .AnyAsync(b => b.BrandId == request.BrandId);

            if (!brandExists)
            {
                return BadRequest("The selected brand does not exist.");
            }

            // Remove duplicate feature IDs.
            List<int> featureIds = request.FeatureIds.Distinct().ToList();

            // Check that every requested feature exists.
            int existingFeatureCount = await _context.Features
                .CountAsync(f => featureIds.Contains(f.FeatureId));

            if (existingFeatureCount != featureIds.Count)
            {
                return BadRequest("One or more selected features do not exist.");
            }

            // Copy the request information into a database entity.
            Earbud earbud = new Earbud
            {
                Name = request.Name,
                Price = request.Price,
                BatteryLife = request.BatteryLife,
                Rating = request.Rating,
                NoiseCancellation = request.NoiseCancellation,
                TransparencyMode = request.TransparencyMode,
                WaterResistance = request.WaterResistance,
                BluetoothVersion = request.BluetoothVersion,
                BrandId = request.BrandId
            };

            // Connect the earbud to each selected feature.
            foreach (int featureId in featureIds)
            {
                earbud.EarbudFeatures.Add(new EarbudFeature
                {
                    FeatureId = featureId
                });
            }

            // Save the earbud and its feature relationships.
            _context.Earbuds.Add(earbud);
            await _context.SaveChangesAsync();

            // Return HTTP 201 Created and the URL of the new earbud.
            return CreatedAtAction(
                nameof(GetEarbud),
                new { id = earbud.EarbudId },
                new { earbud.EarbudId }
            );
        }

        // PUT: /api/earbuds/1
        // Updates an existing earbud and its feature relationships.
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEarbud(
            int id, EarbudRequest request)
        {
            // Find the existing earbud and load its feature relationships.
            Earbud? earbud = await _context.Earbuds
                .Include(e => e.EarbudFeatures)
                .FirstOrDefaultAsync(e => e.EarbudId == id);

            // Return 404 if the earbud does not exist.
            if (earbud == null)
            {
                return NotFound();
            }

            // Check whether the requested brand exists.
            bool brandExists = await _context.Brands
                .AnyAsync(b => b.BrandId == request.BrandId);

            if (!brandExists)
            {
                return BadRequest("The selected brand does not exist.");
            }

            // Remove duplicate feature IDs and validate them.
            List<int> featureIds = request.FeatureIds.Distinct().ToList();

            int existingFeatureCount = await _context.Features
                .CountAsync(f => featureIds.Contains(f.FeatureId));

            if (existingFeatureCount != featureIds.Count)
            {
                return BadRequest("One or more selected features do not exist.");
            }

            // Update the earbud's properties.
            earbud.Name = request.Name;
            earbud.Price = request.Price;
            earbud.BatteryLife = request.BatteryLife;
            earbud.Rating = request.Rating;
            earbud.NoiseCancellation = request.NoiseCancellation;
            earbud.TransparencyMode = request.TransparencyMode;
            earbud.WaterResistance = request.WaterResistance;
            earbud.BluetoothVersion = request.BluetoothVersion;
            earbud.BrandId = request.BrandId;

            // Remove the earbud's previous feature relationships.
            _context.EarbudFeatures.RemoveRange(earbud.EarbudFeatures);

            // Add the updated feature relationships.
            foreach (int featureId in featureIds)
            {
                _context.EarbudFeatures.Add(new EarbudFeature
                {
                    EarbudId = earbud.EarbudId,
                    FeatureId = featureId
                });
            }

            // Save the changes to SQLite.
            await _context.SaveChangesAsync();

            // Return HTTP 204 No Content.
            return NoContent();
        }

        // DELETE: /api/earbuds/1
        // Deletes an existing earbud from the database.
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEarbud(int id)
        {
            // Search for the earbud using its ID.
            Earbud? earbud = await _context.Earbuds.FindAsync(id);

            // Return HTTP 404 if the earbud does not exist.
            if (earbud == null)
            {
                return NotFound();
            }

            // Mark the earbud for deletion.
            _context.Earbuds.Remove(earbud);

            // Save the deletion to the SQLite database.
            await _context.SaveChangesAsync();

            // Return HTTP 204 to indicate successful deletion.
            return NoContent();
        }
    }
}