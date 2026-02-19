using GestionAnomalies.Entities;
using GestionAnomalies.Entities.Enums;

namespace GestionAnomalies.Services.Interfaces
{
    public interface IAutorisationService
    {
        // Vérification des permissions selon le rôle
        bool PeutCreerAnomalie(string role);
        Task<bool> PeutModifierAnomalieAsync(string role, Anomalie anomalie, int userId);
        Task<bool> PeutSupprimerAnomalieAsync(string role, Anomalie anomalie, int userId);
        bool PeutAssignerTechnicien(string role);
        Task<bool> PeutChangerStatutAsync(string role, Anomalie anomalie, int userId);
        bool PeutGererUtilisateurs(string role);
        bool PeutGererReferentiels(string role);
        bool PeutVoirTousLesProjets(string role);
    }
}
