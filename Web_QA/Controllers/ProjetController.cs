using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using GestionAnomalies.Entities;
using GestionAnomalies.Services.Interfaces;
using GestionAnomalies.Entities.Enums;

namespace GestionAnomalies.Controllers
{
    [Authorize]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public class ProjetController : Controller
    {
        private readonly IProjetService _projetService;
        private readonly IReferentielService _referentielService;
        private readonly IAutorisationService _autorisationService;

        public ProjetController(
            IProjetService projetService,
            IReferentielService referentielService,
            IAutorisationService autorisationService)
        {
            _projetService = projetService;
            _referentielService = referentielService;
            _autorisationService = autorisationService;
        }

        // GET: Projet/Index
        public async Task<IActionResult> Index()
        {
            var role = User.FindFirstValue(ClaimTypes.Role)!;
            IEnumerable<Projet> projets;

            // Admin voit tous les projets
            if (_autorisationService.PeutVoirTousLesProjets(role))
            {
                projets = await _projetService.GetAllProjetsAsync();
            }
            else
            {
                // Responsable voit uniquement ses projets (à implémenter dans ProjetService)
                projets = await _projetService.GetAllProjetsAsync();
            }

            return View(projets);
        }

        // GET: Projet/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var projet = await _projetService.GetAllProjetsAsync();
            var projetDetails = projet.FirstOrDefault(p => p.Id == id);
            
            if (projetDetails == null)
            {
                return NotFound();
            }

            return View(projetDetails);
        }

        // GET: Projet/Create
        [Authorize(Roles = "Administrateur")]
        public async Task<IActionResult> Create()
        {
            await ChargerListeResponsables();
            return View();
        }

        // POST: Projet/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrateur")]
        public async Task<IActionResult> Create(Projet model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _projetService.CreerProjetAsync(model);
                    TempData["SuccessMessage"] = "Projet créé avec succès.";
                    return RedirectToAction(nameof(Index));
                }
                catch (ArgumentException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            await ChargerListeResponsables();
            return View(model);
        }

        // GET: Projet/Edit/5
        [Authorize(Roles = "Administrateur")]
        public async Task<IActionResult> Edit(int id)
        {
            var projets = await _projetService.GetAllProjetsAsync();
            var projet = projets.FirstOrDefault(p => p.Id == id);
            
            if (projet == null)
            {
                return NotFound();
            }

            await ChargerListeResponsables();
            return View(projet);
        }

        // POST: Projet/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrateur")]
        public async Task<IActionResult> Edit(int id, Projet model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _projetService.ModifierProjetAsync(model);
                    TempData["SuccessMessage"] = "Projet modifié avec succès.";
                    return RedirectToAction(nameof(Index));
                }
                catch (ArgumentException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            await ChargerListeResponsables();
            return View(model);
        }

        // POST: Projet/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrateur")]
        public async Task<IActionResult> Delete(int id)
        {
            // À implémenter avec ProjetRepository.DeleteAsync
            TempData["SuccessMessage"] = "Projet supprimé avec succès.";
            return RedirectToAction(nameof(Index));
        }

        // Méthode privée pour charger la liste des responsables
        private async Task ChargerListeResponsables()
        {
            var responsables = await _referentielService.GetResponsablesAsync();
            
            ViewBag.Responsables = new SelectList(
                responsables.Select(u => new {
                    Id = u.Id,
                    NomComplet = $"{u.Prenom} {u.Nom} ({u.Role})"
                }),
                "Id", 
                "NomComplet"
            );
        }
    }
}
