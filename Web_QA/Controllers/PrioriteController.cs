using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GestionAnomalies.Repositories.Interfaces;
using GestionAnomalies.Entities;

namespace GestionAnomalies.Controllers
{
    [Authorize(Roles = "Administrateur")]
    public class PrioriteController : Controller
    {
        private readonly IPrioriteRepository _prioriteRepo;

        public PrioriteController(IPrioriteRepository prioriteRepo)
        {
            _prioriteRepo = prioriteRepo;
        }

        // GET: Priorite
        public async Task<IActionResult> Index()
        {
            var priorites = await _prioriteRepo.GetAllAsync();
            return View(priorites);
        }

        // GET: Priorite/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Priorite/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Priorite priorite)
        {
            if (ModelState.IsValid)
            {
                priorite.DateCreation = DateTime.Now;
                await _prioriteRepo.AddAsync(priorite);
                TempData["SuccessMessage"] = "Priorité créée avec succès.";
                return RedirectToAction(nameof(Index));
            }
            return View(priorite);
        }

        // GET: Priorite/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var priorite = await _prioriteRepo.GetByIdAsync(id);
            if (priorite == null)
            {
                return NotFound();
            }
            return View(priorite);
        }

        // POST: Priorite/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Priorite priorite)
        {
            if (id != priorite.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                priorite.DateModification = DateTime.Now;
                await _prioriteRepo.UpdateAsync(priorite);
                TempData["SuccessMessage"] = "Priorité modifiée avec succès.";
                return RedirectToAction(nameof(Index));
            }
            return View(priorite);
        }

        // POST: Priorite/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _prioriteRepo.DeleteAsync(id);
            TempData["SuccessMessage"] = "Priorité supprimée avec succès.";
            return RedirectToAction(nameof(Index));
        }
    }
}
