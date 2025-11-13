using BeFit.Data;
using BeFit.Helpers;
using BeFit.Models.BeFit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BeFit.Controllers
{
    [Authorize]
    public class PerformedExercisesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PerformedExercisesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Lista wszystkich ćwiczeń wykonanych przez użytkownika
        public async Task<IActionResult> Index()
        {
            var userId = User.GetUserId();
            var data = await _context.PerformedExercises
                .Include(p => p.TrainingSession)
                .Include(p => p.ExerciseType)
                .Where(p => p.UserId == userId)
                .OrderByDescending(p => p.TrainingSession.Start)
                .ToListAsync();

            return View(data);
        }

        public async Task<IActionResult> Create(int? sessionId)
        {
            var userId = User.GetUserId();

            ViewData["ExerciseTypeId"] = new SelectList(
                await _context.ExerciseTypes.ToListAsync(), "Id", "Name");

            ViewData["TrainingSessionId"] = new SelectList(
                await _context.TrainingSessions
                    .Where(s => s.UserId == userId)
                    .OrderByDescending(s => s.Start)
                    .ToListAsync(),
                "Id", "Start");

            var model = new PerformedExercise();
            if (sessionId.HasValue)
                model.TrainingSessionId = sessionId.Value;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PerformedExercise model)
        {
            var userId = User.GetUserId();
            model.UserId = userId;

            // Sprawdzamy, czy sesja należy do użytkownika
            bool ownsSession = await _context.TrainingSessions
                .AnyAsync(s => s.Id == model.TrainingSessionId && s.UserId == userId);

            if (!ownsSession)
            {
                ModelState.AddModelError(string.Empty, "Nie możesz przypisać ćwiczenia do cudzej sesji.");
            }

            if (!ModelState.IsValid)
            {
                await FillSelectLists(userId);
                return View(model);
            }

            _context.Add(model);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var userId = User.GetUserId();

            var ex = await _context.PerformedExercises
                .FirstOrDefaultAsync(p => p.Id == id && p.UserId == userId);
            if (ex == null) return NotFound();

            await FillSelectLists(userId);
            return View(ex);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PerformedExercise model)
        {
            var userId = User.GetUserId();

            if (id != model.Id) return NotFound();

            // upewniamy się, że ćwiczenie należy do użytkownika
            var existing = await _context.PerformedExercises
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id && p.UserId == userId);
            if (existing == null) return Forbid();

            model.UserId = userId;

            // ponowna walidacja przynależności sesji
            bool ownsSession = await _context.TrainingSessions
                .AnyAsync(s => s.Id == model.TrainingSessionId && s.UserId == userId);
            if (!ownsSession)
            {
                ModelState.AddModelError(string.Empty, "Nie możesz przypisać ćwiczenia do cudzej sesji.");
            }

            if (!ModelState.IsValid)
            {
                await FillSelectLists(userId);
                return View(model);
            }

            _context.Update(model);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var userId = User.GetUserId();

            var ex = await _context.PerformedExercises
                .Include(p => p.ExerciseType)
                .Include(p => p.TrainingSession)
                .FirstOrDefaultAsync(p => p.Id == id && p.UserId == userId);

            if (ex == null) return NotFound();
            return View(ex);
        }

        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userId = User.GetUserId();

            var performedExercise = await _context.PerformedExercises
                .Include(p => p.TrainingSession)
                .Include(p => p.ExerciseType)
                .FirstOrDefaultAsync(p => p.Id == id && p.UserId == userId);

            if (performedExercise == null)
            {
                // jeśli chcesz, możesz tu dać np. RedirectToAction("Index")
                return NotFound();
            }

            return View(performedExercise);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = User.GetUserId();

            var ex = await _context.PerformedExercises
                .FirstOrDefaultAsync(p => p.Id == id && p.UserId == userId);

            if (ex != null)
            {
                _context.PerformedExercises.Remove(ex);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private async Task FillSelectLists(string userId)
        {
            ViewData["ExerciseTypeId"] = new SelectList(
                await _context.ExerciseTypes.ToListAsync(), "Id", "Name");

            ViewData["TrainingSessionId"] = new SelectList(
                await _context.TrainingSessions
                    .Where(s => s.UserId == userId)
                    .OrderByDescending(s => s.Start)
                    .ToListAsync(),
                "Id", "Start");
        }
    }
}
