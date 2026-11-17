using EarbudExplorer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Packet_Sniffers.Data;
using Microsoft.AspNetCore.Authorization;

namespace Packet_Sniffers.Controllers
{
    // Handles HTTP requests for brand information.
    [ApiController]
    [Route("api/brands")]
    public class BrandsApiController : ControllerBase
    {
        // Provides access to the application's database.
        private readonly ApplicationDbContext _context;

        // Receives the database context through dependency injection.
        public BrandsApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /api/brands
        // Returns all brands as JSON.
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Brand>>> GetBrands()
        {
            return await _context.Brands.ToListAsync();
        }

        // GET: /api/brands/1
        // Retrieves one brand using its unique ID.
        // id: The ID of the brand being requested.
        [HttpGet("{id}")]
        public async Task<ActionResult<Brand>> GetBrand(int id)
        {
            // Search the Brands table for a matching primary key.
            Brand? brand = await _context.Brands.FindAsync(id);

            // Return HTTP 404 if the brand does not exist.
            if (brand == null)
            {
                return NotFound();
            }

            // Return HTTP 200 with the brand as JSON.
            return Ok(brand);
        }

        // POST: /api/brands
        // Creates a new brand in the database.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<Brand>> CreateBrand(Brand brand)
        {
            // Add the new brand to the database context.
            _context.Brands.Add(brand);

            // Save the new brand to the database.
            await _context.SaveChangesAsync();

            // Return HTTP 201 Created with the new brand.
            return CreatedAtAction(
                nameof(GetBrand),
                new { id = brand.BrandId },
                brand
            );
        }


        // PUT: /api/brands/1
        // Updates an existing brand.
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBrand(int id, Brand brand)
        {
            // Find the brand that needs to be updated.
            Brand? existingBrand = await _context.Brands.FindAsync(id);

            // Return HTTP 404 if the brand does not exist.
            if (existingBrand == null)
            {
                return NotFound();
            }

            // Update the existing brand's name.
            existingBrand.Name = brand.Name;

            // Save the changes to the database.
            await _context.SaveChangesAsync();

            // Return HTTP 204 because the update succeeded.
            return NoContent();
        }


        // DELETE: /api/brands/2
        // Deletes a brand only if no earbuds belong to it.
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBrand(int id)
        {
            // Find the brand in the database.
            Brand? brand = await _context.Brands.FindAsync(id);

            // Return 404 if the brand does not exist.
            if (brand == null)
            {
                return NotFound();
            }

            // Check whether any earbuds belong to this brand.
            bool hasEarbuds = await _context.Earbuds
                .AnyAsync(e => e.BrandId == id);

            // Prevent deleting the brand if it has earbuds.
            if (hasEarbuds)
            {
                return Conflict(
                    "Cannot delete this brand because it has associated earbuds."
                );
            }

            // Delete the brand only when it has no earbuds.
            _context.Brands.Remove(brand);
            await _context.SaveChangesAsync();

            // Return HTTP 204 after successful deletion.
            return NoContent();
        }
    }
}