using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GestionAnomalies.Models;
using GestionAnomalies.Repositories.Interfaces;

namespace GestionAnomalies.Controllers
{
    [Authorize] // Protège toutes les actions du contrôleur
    public class HomeController : Controller
    {
        private readonly IAnomalieRepository _anomalieRepo;

        public HomeController(
            IAnomalieRepository anomalieRepo)
        {
            _anomalieRepo = anomalieRepo;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> Index()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value ?? "Utilisateur";
            var userName = User.FindFirst(ClaimTypes.Name)?.Value ?? "Utilisateur";

            var model = new DashboardViewModel
            {
                NomUtilisateur = userName,
                Role = userRole
            };

            if (!string.IsNullOrEmpty(userIdClaim) && int.TryParse(userIdClaim, out int userId))
            {
                // Récupérer toutes les anomalies
                var anomalies = await _anomalieRepo.GetAllAsync();
                
                // Statistiques globales
                model.TotalAnomalies = anomalies.Count();
                model.AnomaliesRecentes = anomalies.Take(5);

                // Anomalies par statut
                var anomaliesParStatut = anomalies.GroupBy(a => a.Statut?.Libelle ?? "Non défini")
                    .ToDictionary(g => g.Key, g => g.Count());
                model.AnomaliesParStatut = anomaliesParStatut;

                // Anomalies par technicien
                var anomaliesParTechnicien = anomalies
                    .Where(a => a.Assignee != null)
                    .GroupBy(a => a.Assignee!.Nom + " " + a.Assignee.Prenom)
                    .ToDictionary(g => g.Key, g => g.Count());
                
                // Ajouter les anomalies non assignées
                var nonAssignees = anomalies.Count(a => a.AssigneeId == null);
                if (nonAssignees > 0)
                {
                    anomaliesParTechnicien.Add("Non assignées", nonAssignees);
                }
                model.AnomaliesParTechnicien = anomaliesParTechnicien;

                // Anomalies par priorité
                var anomaliesParPriorite = anomalies.GroupBy(a => a.Priorite?.Libelle ?? "Non définie")
                    .ToDictionary(g => g.Key, g => g.Count());
                model.AnomaliesParPriorite = anomaliesParPriorite;

                // Temps moyen de résolution (en jours)
                var anomaliesResolues = anomalies
                    .Where(a => a.DateCloture.HasValue)
                    .Select(a => (a.DateCloture!.Value - a.DateCreation).TotalDays)
                    .ToList();
                
                model.TempsMoyenResolution = anomaliesResolues.Any() 
                    ? Math.Round(anomaliesResolues.Average(), 2) 
                    : 0;

                // Statistiques existantes
                model.AnomaliesNonAssignees = anomalies.Where(a => a.AssigneeId == null).Take(5);
                model.MesAnomaliesAssignees = anomalies.Where(a => a.AssigneeId == userId).Take(5);
            }

            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
