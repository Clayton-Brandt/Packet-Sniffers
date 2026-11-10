// using Microsoft.AspNetCore.Mvc; 
// using Holistic_Hiking.Models.Entities;
// using Holistic_Hiking.Services;

// namespace Holistic_Hiking.Controllers;

// public class TrailsController : Controller
// {
//     private readonly ITrailRepository _trailRepo;

//     public TrailsController (ITrailRepository trailRepo)
//     {
//         _trailRepo = trailRepo;
//     }


//     public async Task<IActionResult> Index(
//         string? searchString,
//         string? difficulty,
//         string? lengthRange,
//         int? rating)
//     {
//         // Retrieve all trails from the database
//         var trails = await _trailRepo.ReadAllAsync();

//         // Search trails by name
//         if (!string.IsNullOrWhiteSpace(searchString))
//         {
//             trails = trails.Where(t =>
//                 t.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase));
//         }

//         // Filter by difficulty
//         if (!string.IsNullOrWhiteSpace(difficulty))
//         {
//             trails = trails.Where(t => t.Difficulty == difficulty);
//         }

//         // Filter by length
//         if (!string.IsNullOrWhiteSpace(lengthRange))
//         {
//             trails = lengthRange switch
//             {
//                 "under5" => trails.Where(t => t.Length < 5),
//                 "5to10" => trails.Where(t => t.Length >= 5 && t.Length <= 10),
//                 "over10" => trails.Where(t => t.Length > 10),
//                 _ => trails
//             };
//         }

//         // Filter by rating
//         if (rating.HasValue)
//         {
//             trails = trails.Where(t => t.Rating == rating.Value);
//         }

//         // Preserve selected filters when the page reloads
//         ViewBag.SearchString = searchString;
//         ViewBag.Difficulty = difficulty;
//         ViewBag.LengthRange = lengthRange;
//         ViewBag.Rating = rating;

//         return View(trails);
//     }


//     public async Task<IActionResult> Details (int id)
//     {
//         var trail = await _trailRepo.ReadAsync(id);
//         if (trail == null)
//         {
//             return NotFound();
//         }
//         return View(trail);
//     }
// }