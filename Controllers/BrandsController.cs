
using EarbudExplorer.Data;
using EarbudExplorer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EarbudExplorer.Controllers
{
    // Handles requests related to earbud manufacturers.
    // Provides Create, Read, Update, and Delete functionality.
    public class BrandsController : Controller
    {
        // Database context used to access the Brands table.
        private readonly ApplicationDbContext _context;

        // Constructor receives the database context
        // through ASP.NET Core dependency injection.
        public BrandsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Brands
        // Retrieves all brands and displays them in a list.
        public async Task<IActionResult> Index()
        {
            var brands = await _context.Brands
                .OrderBy(b => b.Name)
                .ToListAsync();

            return View(brands);
        }

        // GET: /Brands/Details/5
        // Displays information about one brand.
        public async Task<IActionResult> Details(int? id)
        {
            // A brand ID must be provided.
            if (id == null)
            {
                return NotFound();
            }

            // Search for the brand with the requested ID.
            var brand = await _context.Brands
                .FirstOrDefaultAsync(b => b.BrandId == id);

            // Return 404 if the brand does not exist.
            if (brand == null)
            {
                return NotFound();
            }

            return View(brand);
        }

        // GET: /Brands/Create
        // Displays the form for adding a new brand.
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Brands/Create
        // Receives the submitted form and saves a new brand.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Name")] Brand brand)
        {
            // Check validation attributes in the Brand model.
            if (ModelState.IsValid)
            {
                // Add the new brand to the database.
                _context.Brands.Add(brand);

                // Save the changes to SQL Server.
                await _context.SaveChangesAsync();

                // Return to the list of brands.
                return RedirectToAction(nameof(Index));
            }

            // Redisplay the form if validation fails.
            return View(brand);
        }

        // GET: /Brands/Edit/5
        // Displays a form containing an existing brand's information.
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Find the brand using its primary key.
            var brand = await _context.Brands.FindAsync(id);

            if (brand == null)
            {
                return NotFound();
            }

            return View(brand);
        }

        // POST: /Brands/Edit/5
        // Updates the name of an existing brand.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id, [Bind("BrandId,Name")] Brand brand)
        {
            // Ensure the URL ID matches the submitted brand ID.
            if (id != brand.BrandId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // Find the existing database record.
                var existingBrand = await _context.Brands
                    .FindAsync(id);

                if (existingBrand == null)
                {
                    return NotFound();
                }

                // Update only the brand name.
                existingBrand.Name = brand.Name;

                // Save the updated value to SQL Server.
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(brand);
        }

        // GET: /Brands/Delete/5
        // Displays a confirmation page before deleting a brand.
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Find the brand to be deleted.
            var brand = await _context.Brands
                .FirstOrDefaultAsync(b => b.BrandId == id);

            if (brand == null)
            {
                return NotFound();
            }

            return View(brand);
        }

        // POST: /Brands/Delete/5
        // Deletes the brand after the user confirms.
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Find the brand in the database.
            var brand = await _context.Brands.FindAsync(id);

            if (brand == null)
            {
                return NotFound();
            }

            // Check whether any earbuds belong to this brand.
            // A brand with earbuds should not be deleted.
            bool hasEarbuds = await _context.Earbuds
                .AnyAsync(e => e.BrandId == id);

            if (hasEarbuds)
            {
                // Display an error on the confirmation page.
                ModelState.AddModelError(
                    "",
                    "This brand cannot be deleted because it has earbuds."
                );

                return View("Delete", brand);
            }

            // Remove the brand when no earbuds reference it.
            _context.Brands.Remove(brand);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
