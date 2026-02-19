using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GestionAnomalies.Repositories.Interfaces;
using GestionAnomalies.Entities;

namespace GestionAnomalies.Controllers
{
    [Authorize(Roles = "Administrateur")]
    public class TypeAnomalieController : Controller
    {
        private readonly ITypeAnomalieRepository _typeRepo;

        public TypeAnomalieController(ITypeAnomalieRepository typeRepo)
        {
            _typeRepo = typeRepo;
        }

        // GET: TypeAnomalie
        public async Task<IActionResult> Index()
        {
            var types = await _typeRepo.GetAllAsync();
            return View(types);
        }

        // GET: TypeAnomalie/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TypeAnomalie/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TypeAnomalie type)
        {
            if (ModelState.IsValid)
            {
                type.DateCreation = DateTime.Now;
                await _typeRepo.AddAsync(type);
                TempData["SuccessMessage"] = "Type d'anomalie créé avec succès.";
                return RedirectToAction(nameof(Index));
            }
            return View(type);
        }

        // GET: TypeAnomalie/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var type = await _typeRepo.GetByIdAsync(id);
            if (type == null)
            {
                return NotFound();
            }
            return View(type);
        }

        // POST: TypeAnomalie/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TypeAnomalie type)
        {
            if (id != type.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                type.DateModification = DateTime.Now;
                await _typeRepo.UpdateAsync(type);
                TempData["SuccessMessage"] = "Type d'anomalie modifié avec succès.";
                return RedirectToAction(nameof(Index));
            }
            return View(type);
        }

        // POST: TypeAnomalie/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _typeRepo.DeleteAsync(id);
            TempData["SuccessMessage"] = "Type d'anomalie supprimé avec succès.";
            return RedirectToAction(nameof(Index));
        }
    }
}
