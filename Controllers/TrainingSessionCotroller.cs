using BeFit.Data;
using BeFit.Helpers;
using BeFit.Models.BeFit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BeFit.Controllers
{
    [Authorize]   // tylko zalogowani
    public class TrainingSessionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TrainingSessionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Lista sesji aktualnego użytkownika
        public async Task<IActionResult> Index()
        {
            var userId = User.GetUserId();
            var sessions = await _context.TrainingSessions
                .Where(s => s.UserId == userId)
                .OrderByDescending(s => s.Start)
                .ToListAsync();

            return View(sessions);
        }

        public IActionResult Create()
        {
            return View(new TrainingSession
            {
                Start = DateTime.Now,
                End = DateTime.Now.AddHours(1)
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TrainingSession model)
        {
            // UserId ustawiamy automatycznie
            model.UserId = User.GetUserId();

            if (!ModelState.IsValid)
                return View(model);

            _context.Add(model);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var userId = User.GetUserId();

            var session = await _context.TrainingSessions
                .FirstOrDefaultAsync(s => s.Id == id && s.UserId == userId);

            if (session == null) return NotFound();
            return View(session);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TrainingSession model)
        {
            var userId = User.GetUserId();

            if (id != model.Id) return NotFound();

            // upewniamy się, że sesja należy do użytkownika
            var existing = await _context.TrainingSessions
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == id && s.UserId == userId);

            if (existing == null) return Forbid();

            model.UserId = userId;

            if (!ModelState.IsValid)
                return View(model);

            _context.Update(model);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var userId = User.GetUserId();

            var session = await _context.TrainingSessions
                .FirstOrDefaultAsync(s => s.Id == id && s.UserId == userId);

            if (session == null) return NotFound();
            return View(session);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var userId = User.GetUserId();

            var session = await _context.TrainingSessions
                .FirstOrDefaultAsync(s => s.Id == id && s.UserId == userId);

            if (session == null) return NotFound();
            return View(session);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = User.GetUserId();

            var session = await _context.TrainingSessions
                .FirstOrDefaultAsync(s => s.Id == id && s.UserId == userId);

            if (session != null)
            {
                _context.TrainingSessions.Remove(session);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
