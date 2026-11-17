using EarbudExplorer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Packet_Sniffers.Data;
using Microsoft.AspNetCore.Authorization;

namespace Packet_Sniffers.Controllers
{
    // Handles HTTP requests for earbud features.
    [ApiController]
    [Route("api/features")]
    public class FeaturesApiController : ControllerBase
    {
        // Provides access to the SQLite database.
        private readonly ApplicationDbContext _context;

        // Receives the database context through dependency injection.
        public FeaturesApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /api/features
        // Returns all available features as JSON.
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Feature>>> GetFeatures()
        {
            return await _context.Features.ToListAsync();
        }

        // GET: /api/features/1
        // Returns one feature by its ID.
        [HttpGet("{id}")]

        public async Task<ActionResult<Feature>> GetFeature(int id)
        {
            // Find the requested feature.
            Feature? feature = await _context.Features.FindAsync(id);

            // Return 404 if the feature does not exist.
            if (feature == null)
            {
                return NotFound();
            }

            // Return 200 OK with the feature as JSON.
            return Ok(feature);
        }

        // POST: /api/features
        // Creates a new feature in the database.
        [Authorize(Roles = "Admin")]
        [HttpPost]

        public async Task<ActionResult<Feature>> CreateFeature(Feature feature)
        {
            // Add the new feature to the database context.
            _context.Features.Add(feature);

            // Save the feature to SQLite.
            await _context.SaveChangesAsync();

            // Return HTTP 201 Created with the new feature.
            return CreatedAtAction(
                nameof(GetFeature),
                new { id = feature.FeatureId },
                feature
            );
        }


        // PUT: /api/features/1
        // Updates an existing feature.
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFeature(int id, Feature feature)
        {
            // Find the feature being updated.
            Feature? existingFeature = await _context.Features.FindAsync(id);

            // Return HTTP 404 if the feature does not exist.
            if (existingFeature == null)
            {
                return NotFound();
            }

            // Update only the editable properties.
            existingFeature.Name = feature.Name;
            existingFeature.Description = feature.Description;

            // Save changes to SQLite.
            await _context.SaveChangesAsync();

            // Return HTTP 204 No Content.
            return NoContent();
        }


        // DELETE: /api/features/1
        // Deletes an existing feature.
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFeature(int id)
        {
            // Find the feature being deleted.
            Feature? feature = await _context.Features.FindAsync(id);

            // Return HTTP 404 if it doesn't exist.
            if (feature == null)
            {
                return NotFound();
            }

            // Remove the feature.
            _context.Features.Remove(feature);

            // Save the deletion.
            await _context.SaveChangesAsync();

            // Return HTTP 204 No Content.
            return NoContent();
        }
    }
}