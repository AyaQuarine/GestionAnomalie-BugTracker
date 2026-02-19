using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using GestionAnomalies.Entities;
using GestionAnomalies.Services.Interfaces;

namespace GestionAnomalies.Controllers
{
    [Authorize]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public class AnomalieController : Controller
    {
        private readonly IAnomalieService _anomalieService;
        private readonly IProjetService _projetService;
        private readonly IReferentielService _referentielService;
        private readonly IAutorisationService _autorisationService;
        private readonly ICommentaireService _commentaireService;
        private readonly IPieceJointeService _pieceJointeService;

        public AnomalieController(
            IAnomalieService anomalieService,
            IProjetService projetService,
            IReferentielService referentielService,
            IAutorisationService autorisationService,
            ICommentaireService commentaireService,
            IPieceJointeService pieceJointeService)
        {
            _anomalieService = anomalieService;
            _projetService = projetService;
            _referentielService = referentielService;
            _autorisationService = autorisationService;
            _commentaireService = commentaireService;
            _pieceJointeService = pieceJointeService;
        }

        // GET: Anomalie/Index
        public async Task<IActionResult> Index()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var role = User.FindFirstValue(ClaimTypes.Role)!;

            var anomalies = await _anomalieService.GetAnomaliesPourUtilisateurAsync(userId, role);
            
            return View(anomalies);
        }

        // GET: Anomalie/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var anomalie = await _anomalieService.GetDetailsAsync(id);
            
            if (anomalie == null)
            {
                return NotFound();
            }

            var commentaires = await _commentaireService.GetByAnomalieIdAsync(id);
            var piecesJointes = await _pieceJointeService.GetByAnomalieIdAsync(id);

            // Charger les listes déroulantes pour les actions
            var techniciens = await _referentielService.GetTechniciensAsync();
            var statuts = await _referentielService.GetStatutsAsync();

            ViewBag.Commentaires = commentaires;
            ViewBag.PiecesJointes = piecesJointes;
            ViewBag.UserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            ViewBag.UserRole = User.FindFirstValue(ClaimTypes.Role)!;
            
            ViewBag.Techniciens = new SelectList(
                techniciens.Select(t => new {
                    Id = t.Id,
                    NomComplet = $"{t.Prenom} {t.Nom}"
                }),
                "Id",
                "NomComplet",
                anomalie.AssigneeId
            );
            
            ViewBag.Statuts = new SelectList(
                statuts,
                "Id",
                "Libelle",
                anomalie.StatutId
            );

            return View(anomalie);
        }

        // GET: Anomalie/Create
        public async Task<IActionResult> Create()
        {
            var role = User.FindFirstValue(ClaimTypes.Role)!;

            if (!_autorisationService.PeutCreerAnomalie(role))
            {
                return Forbid();
            }

            await ChargerListesDeroulantes();
            return View();
        }

        // POST: Anomalie/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Anomalie model)
        {
            var role = User.FindFirstValue(ClaimTypes.Role)!;

            if (!_autorisationService.PeutCreerAnomalie(role))
            {
                return Forbid();
            }

            if (ModelState.IsValid)
            {
                model.CreateurId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                await _anomalieService.CreerAnomalieAsync(model);
                
                TempData["SuccessMessage"] = "Anomalie créée avec succès.";
                return RedirectToAction(nameof(Index));
            }

            await ChargerListesDeroulantes();
            return View(model);
        }

        // GET: Anomalie/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var anomalie = await _anomalieService.GetDetailsAsync(id);
            
            if (anomalie == null)
            {
                return NotFound();
            }

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var role = User.FindFirstValue(ClaimTypes.Role)!;

            if (!await _autorisationService.PeutModifierAnomalieAsync(role, anomalie, userId))
            {
                TempData["ErrorMessage"] = "Vous n'êtes pas autorisé à modifier cette anomalie.";
                return RedirectToAction(nameof(Index));
            }

            await ChargerListesDeroulantes();
            return View(anomalie);
        }

        // POST: Anomalie/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Anomalie model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            var anomalie = await _anomalieService.GetDetailsAsync(id);
            if (anomalie == null)
            {
                return NotFound();
            }

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var role = User.FindFirstValue(ClaimTypes.Role)!;

            if (!await _autorisationService.PeutModifierAnomalieAsync(role, anomalie, userId))
            {
                return Forbid();
            }

            if (ModelState.IsValid)
            {
                anomalie.Titre = model.Titre;
                anomalie.Description = model.Description;
                anomalie.ProjetId = model.ProjetId;
                anomalie.TypeAnomalieId = model.TypeAnomalieId;
                anomalie.PrioriteId = model.PrioriteId;
                anomalie.DateModification = DateTime.Now;

                await _anomalieService.CreerAnomalieAsync(anomalie); // Utilise le même repo update
                
                TempData["SuccessMessage"] = "Anomalie modifiée avec succès.";
                return RedirectToAction(nameof(Details), new { id });
            }

            await ChargerListesDeroulantes();
            return View(model);
        }

        // POST: Anomalie/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var anomalie = await _anomalieService.GetDetailsAsync(id);
            
            if (anomalie == null)
            {
                return NotFound();
            }

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var role = User.FindFirstValue(ClaimTypes.Role)!;

            if (!await _autorisationService.PeutSupprimerAnomalieAsync(role, anomalie, userId))
            {
                TempData["ErrorMessage"] = "Vous n'êtes pas autorisé à supprimer cette anomalie.";
                return RedirectToAction(nameof(Index));
            }

            // Utiliser le repository (à ajouter dans IAnomalieRepository.DeleteAsync)
            TempData["SuccessMessage"] = "Anomalie supprimée avec succès.";
            return RedirectToAction(nameof(Index));
        }

        // POST: Anomalie/AssignTechnician
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignTechnician(int anomalieId, int technicienId)
        {
            var role = User.FindFirstValue(ClaimTypes.Role)!;

            if (!_autorisationService.PeutAssignerTechnicien(role))
            {
                return Forbid();
            }

            await _anomalieService.AssignerTechnicienAsync(anomalieId, technicienId);
            
            TempData["SuccessMessage"] = "Technicien assigné avec succès.";
            return RedirectToAction(nameof(Details), new { id = anomalieId });
        }

        // POST: Anomalie/ChangeStatus
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeStatus(int anomalieId, int nouveauStatutId)
        {
            var anomalie = await _anomalieService.GetDetailsAsync(anomalieId);
            
            if (anomalie == null)
            {
                return NotFound();
            }

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var role = User.FindFirstValue(ClaimTypes.Role)!;

            if (!await _autorisationService.PeutChangerStatutAsync(role, anomalie, userId))
            {
                TempData["ErrorMessage"] = "Vous n'êtes pas autorisé à changer le statut de cette anomalie.";
                return RedirectToAction(nameof(Details), new { id = anomalieId });
            }

            await _anomalieService.ChangerStatutAsync(anomalieId, nouveauStatutId);
            
            TempData["SuccessMessage"] = "Statut mis à jour avec succès.";
            return RedirectToAction(nameof(Details), new { id = anomalieId });
        }

        // Méthode privée pour charger les listes déroulantes
        private async Task ChargerListesDeroulantes()
        {
            var projets = await _projetService.GetAllProjetsAsync();
            var types = await _referentielService.GetTypesAsync();
            var priorites = await _referentielService.GetPrioritesAsync();
            var statuts = await _referentielService.GetStatutsAsync();
            var techniciens = await _referentielService.GetTechniciensAsync();

            ViewBag.Projets = new SelectList(projets, "Id", "Nom");
            ViewBag.Types = new SelectList(types, "Id", "Libelle");
            ViewBag.Priorites = new SelectList(priorites, "Id", "Libelle");
            ViewBag.Statuts = new SelectList(statuts, "Id", "Libelle");
            ViewBag.Techniciens = new SelectList(techniciens, "Id", "Nom");
        }
    }
}
