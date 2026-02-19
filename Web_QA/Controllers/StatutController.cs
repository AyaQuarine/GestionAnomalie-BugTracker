using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GestionAnomalies.Repositories.Interfaces;
using GestionAnomalies.Entities;

namespace GestionAnomalies.Controllers
{
    [Authorize(Roles = "Administrateur")]
    public class StatutController : Controller
    {
        private readonly IStatutRepository _statutRepo;

        public StatutController(IStatutRepository statutRepo)
        {
            _statutRepo = statutRepo;
        }

        // GET: Statut
        public async Task<IActionResult> Index()
        {
            var statuts = await _statutRepo.GetAllAsync();
            return View(statuts);
        }

        // GET: Statut/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Statut/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Statut statut)
        {
            if (ModelState.IsValid)
            {
                statut.DateCreation = DateTime.Now;
                await _statutRepo.AddAsync(statut);
                TempData["SuccessMessage"] = "Statut créé avec succès.";
                return RedirectToAction(nameof(Index));
            }
            return View(statut);
        }

        // GET: Statut/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var statut = await _statutRepo.GetByIdAsync(id);
            if (statut == null)
            {
                return NotFound();
            }
            return View(statut);
        }

        // POST: Statut/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Statut statut)
        {
            if (id != statut.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                statut.DateModification = DateTime.Now;
                await _statutRepo.UpdateAsync(statut);
                TempData["SuccessMessage"] = "Statut modifié avec succès.";
                return RedirectToAction(nameof(Index));
            }
            return View(statut);
        }

        // POST: Statut/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _statutRepo.DeleteAsync(id);
            TempData["SuccessMessage"] = "Statut supprimé avec succès.";
            return RedirectToAction(nameof(Index));
        }
    }
}
