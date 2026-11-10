// using Microsoft.EntityFrameworkCore;
// using Packet_Sniffers.Models.Entities;
// using Packet_Sniffers.Data;

// namespace Packet_Sniffers.Services;

// public class DbTrailRepository : ITrailRepository
// {
//     private readonly ApplicationDbContext _context;

//     public DbTrailRepository(ApplicationDbContext context)
//     {
//         _context = context;
//     }

//     // Create Trail
//     public async Task<Trail> CreateAsync (Trail trail)
//     {
//         _context.Trails.Add(trail);
//         await _context.SaveChangesAsync();
//         return trail;
//     }

//     // Read One Trail
//     public async Task<Trail?> ReadAsync (int Id)
//     {
//         return await _context.Trails
//             .FirstOrDefaultAsync(t => t.TrailId == Id);
//     }

//     // Read All Trails 
//     public async Task<IEnumerable<Trail>> ReadAllAsync ()
//     {
//         return await _context.Trails.ToListAsync();
//     }

//     // Update Trail
//     public async Task<Trail?> UpdateAsync (int OldId, Trail trail)
//     {
//         var existing = await _context.Trails.FindAsync (OldId);
//         if (existing is null)
//         {
//             return null;
//         }
//         existing.Name = trail.Name;
//         existing.Location = trail.Location;
//         existing.Length = trail.Length;
//         existing.Difficulty = trail.Difficulty;
//         // existing.Rating = trail.Rating;
//         existing.Description = trail.Description;
//         existing.ImageUrl = trail.ImageUrl;
        
//         await _context.SaveChangesAsync();
//         return existing;
//     }

//     // Delete Trail
//     public async Task<bool> DeleteAsync (int Id)
//     {
//         var existing = await _context.Trails.FindAsync(Id);
//         if (existing is null)
//         {
//             return false;
//         }

//         _context.Trails.Remove(existing);
//         await _context.SaveChangesAsync();
//         return true;
//     }
// }