using BeFit.Data;
using BeFit.Models.BeFit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BeFit.Controllers
{
    public class ExerciseTypesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ExerciseTypesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Lista typów ćwiczeń – dostępna dla wszystkich, także niezalogowanych
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var data = await _context.ExerciseTypes.ToListAsync();
            return View(data);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var type = await _context.ExerciseTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (type == null) return NotFound();

            return View(type);
        }

        // Tworzenie/edycja/usuwanie – tylko administrator
        [Authorize(Roles = "Administrator")]
        public IActionResult Create() => View();

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ExerciseType model)
        {
            if (!ModelState.IsValid) return View(model);

            _context.Add(model);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var type = await _context.ExerciseTypes.FindAsync(id);
            if (type == null) return NotFound();
            return View(type);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ExerciseType model)
        {
            if (id != model.Id) return NotFound();
            if (!ModelState.IsValid) return View(model);

            _context.Update(model);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var type = await _context.ExerciseTypes.FirstOrDefaultAsync(m => m.Id == id);
            if (type == null) return NotFound();
            return View(type);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Administrator")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var type = await _context.ExerciseTypes.FindAsync(id);
            if (type != null)
            {
                _context.ExerciseTypes.Remove(type);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
