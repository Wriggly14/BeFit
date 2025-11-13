using BeFit.Data;
using BeFit.Helpers;
using BeFit.Models.BeFit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BeFit.Controllers
{
    [Authorize]
    public class StatsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StatsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.GetUserId();
            var fromDate = DateTime.Now.Date.AddDays(-28);

            var query = _context.PerformedExercises
                .Include(p => p.TrainingSession)
                .Include(p => p.ExerciseType)
                .Where(p => p.UserId == userId &&
                            p.TrainingSession.Start >= fromDate);

            var data = await query
                .GroupBy(p => new { p.ExerciseTypeId, p.ExerciseType.Name })
                .Select(g => new ExerciseStatsViewModel
                {
                    ExerciseName = g.Key.Name,
                    TimesPerformed = g.Count(),
                    TotalReps = g.Sum(x => x.Sets * x.RepsPerSet),
                    AverageLoad = g.Average(x => x.Load),
                    MaxLoad = g.Max(x => x.Load)
                })
                .OrderByDescending(s => s.TimesPerformed)
                .ToListAsync();

            return View(data);
        }
    }
}
