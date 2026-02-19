using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using GestionAnomalies.Services.Interfaces;

namespace GestionAnomalies.Controllers
{
    [Authorize]
    public class CommentaireController : Controller
    {
        private readonly ICommentaireService _commentaireService;

        public CommentaireController(ICommentaireService commentaireService)
        {
            _commentaireService = commentaireService;
        }

        // POST: Commentaire/Add
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(int anomalieId, string contenu)
        {
            if (string.IsNullOrWhiteSpace(contenu))
            {
                TempData["ErrorMessage"] = "Le commentaire ne peut pas être vide.";
                return RedirectToAction("Details", "Anomalie", new { id = anomalieId });
            }

            var auteurId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            
            var success = await _commentaireService.AddAsync(anomalieId, auteurId, contenu);

            if (success)
            {
                TempData["SuccessMessage"] = "Commentaire ajouté avec succès.";
            }
            else
            {
                TempData["ErrorMessage"] = "Erreur lors de l'ajout du commentaire.";
            }

            return RedirectToAction("Details", "Anomalie", new { id = anomalieId });
        }

        // POST: Commentaire/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, int anomalieId)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            
            var success = await _commentaireService.DeleteAsync(id, userId);

            if (success)
            {
                TempData["SuccessMessage"] = "Commentaire supprimé avec succès.";
            }
            else
            {
                TempData["ErrorMessage"] = "Erreur lors de la suppression du commentaire.";
            }

            return RedirectToAction("Details", "Anomalie", new { id = anomalieId });
        }
    }
}
