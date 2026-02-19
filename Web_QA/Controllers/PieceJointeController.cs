using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GestionAnomalies.Services.Interfaces;

namespace GestionAnomalies.Controllers
{
    [Authorize]
    public class PieceJointeController : Controller
    {
        private readonly IPieceJointeService _pieceJointeService;
        private readonly IWebHostEnvironment _environment;

        public PieceJointeController(
            IPieceJointeService pieceJointeService,
            IWebHostEnvironment environment)
        {
            _pieceJointeService = pieceJointeService;
            _environment = environment;
        }

        // POST: PieceJointe/Upload
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(int anomalieId, IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                TempData["ErrorMessage"] = "Veuillez sélectionner un fichier.";
                return RedirectToAction("Details", "Anomalie", new { id = anomalieId });
            }

            try
            {
                var uploadsPath = Path.Combine(_environment.WebRootPath, "uploads");
                var success = await _pieceJointeService.UploadAsync(anomalieId, file, uploadsPath);

                if (success)
                {
                    TempData["SuccessMessage"] = "Fichier téléchargé avec succès.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Erreur lors du téléchargement du fichier.";
                }
            }
            catch (InvalidOperationException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            return RedirectToAction("Details", "Anomalie", new { id = anomalieId });
        }

        // GET: PieceJointe/Download/5
        public async Task<IActionResult> Download(int id)
        {
            try
            {
                var uploadsPath = Path.Combine(_environment.WebRootPath, "uploads");
                var (fileBytes, fileName, contentType) = await _pieceJointeService.DownloadAsync(id, uploadsPath);

                if (fileBytes == null)
                {
                    TempData["ErrorMessage"] = "Fichier introuvable.";
                    return RedirectToAction("Index", "Anomalie");
                }

                return File(fileBytes, contentType, fileName);
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Erreur lors du téléchargement du fichier.";
                return RedirectToAction("Index", "Anomalie");
            }
        }

        // POST: PieceJointe/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, int anomalieId)
        {
            try
            {
                var uploadsPath = Path.Combine(_environment.WebRootPath, "uploads");
                var success = await _pieceJointeService.DeleteAsync(id, uploadsPath);

                if (success)
                {
                    TempData["SuccessMessage"] = "Fichier supprimé avec succès.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Erreur lors de la suppression du fichier.";
                }
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Erreur lors de la suppression du fichier.";
            }

            return RedirectToAction("Details", "Anomalie", new { id = anomalieId });
        }
    }
}
