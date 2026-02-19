using GestionAnomalies.Entities;
using GestionAnomalies.Entities.Enums;
using GestionAnomalies.Repositories.Interfaces;
using GestionAnomalies.Services.Interfaces;

namespace GestionAnomalies.Services
{
    public class AutorisationService : IAutorisationService
    {
        private readonly IProjetRepository _projetRepo;

        public AutorisationService(IProjetRepository projetRepo)
        {
            _projetRepo = projetRepo;
        }

        public bool PeutCreerAnomalie(string role)
        {
            // Tous peuvent créer une anomalie SAUF Technicien (généralement)
            return role == RoleUser.Administrateur.ToString() ||
                   role == RoleUser.Responsable.ToString() ||
                   role == RoleUser.Utilisateur.ToString();
        }

        public async Task<bool> PeutModifierAnomalieAsync(string role, Anomalie anomalie, int userId)
        {
            // Admin peut tout modifier
            if (role == RoleUser.Administrateur.ToString())
                return true;

            // Responsable peut modifier UNIQUEMENT les anomalies de SES projets
            if (role == RoleUser.Responsable.ToString())
            {
                var mesProjets = await _projetRepo.GetByResponsableIdAsync(userId);
                var mesProjetIds = mesProjets.Select(p => p.Id).ToList();
                return mesProjetIds.Contains(anomalie.ProjetId);
            }

            // Utilisateur peut modifier uniquement SES anomalies NON ENCORE PRISES EN CHARGE
            if (role == RoleUser.Utilisateur.ToString())
                return anomalie.CreateurId == userId && anomalie.AssigneeId == null;

            // Technicien ne peut PAS modifier l'anomalie, seulement changer le statut
            return false;
        }

        public async Task<bool> PeutSupprimerAnomalieAsync(string role, Anomalie anomalie, int userId)
        {
            // Seul Admin peut supprimer
            if (role == RoleUser.Administrateur.ToString())
                return true;

            // Utilisateur peut supprimer SA déclaration si elle n'est pas encore assignée
            if (role == RoleUser.Utilisateur.ToString())
                return anomalie.CreateurId == userId && anomalie.AssigneeId == null;

            return false;
        }

        public bool PeutAssignerTechnicien(string role)
        {
            // Seuls Admin et Responsable peuvent assigner
            return role == RoleUser.Administrateur.ToString() ||
                   role == RoleUser.Responsable.ToString();
        }

        public async Task<bool> PeutChangerStatutAsync(string role, Anomalie anomalie, int userId)
        {
            // Admin peut changer tous les statuts
            if (role == RoleUser.Administrateur.ToString())
                return true;

            // Responsable peut changer le statut UNIQUEMENT des anomalies de SES projets
            if (role == RoleUser.Responsable.ToString())
            {
                var mesProjets = await _projetRepo.GetByResponsableIdAsync(userId);
                var mesProjetIds = mesProjets.Select(p => p.Id).ToList();
                return mesProjetIds.Contains(anomalie.ProjetId);
            }

            // Technicien peut changer le statut UNIQUEMENT de SES tickets assignés
            if (role == RoleUser.Technicien.ToString())
                return anomalie.AssigneeId == userId;

            // Utilisateur ne peut JAMAIS changer le statut
            return false;
        }

        public bool PeutGererUtilisateurs(string role)
        {
            // Seul Admin peut gérer les utilisateurs
            return role == RoleUser.Administrateur.ToString();
        }

        public bool PeutGererReferentiels(string role)
        {
            // Seul Admin peut gérer les référentiels (Types, Priorités, Statuts)
            return role == RoleUser.Administrateur.ToString();
        }

        public bool PeutVoirTousLesProjets(string role)
        {
            // Seul Admin voit tous les projets
            // Responsable voit uniquement ses projets affectés
            return role == RoleUser.Administrateur.ToString();
        }
    }
}
