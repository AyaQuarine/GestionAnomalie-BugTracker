using GestionAnomalies.Entities;
using GestionAnomalies.Entities.Enums;

namespace GestionAnomalies.Services.Interfaces
{
    public interface IAnomalieService
    {
        // Récupère les anomalies filtrées selon le rôle de la personne connectée
        Task<IEnumerable<Anomalie>> GetAnomaliesPourUtilisateurAsync(int userId, string role);
        
        Task<Anomalie?> GetDetailsAsync(int id);
        
        // Logique métier : Création avec statut par défaut
        Task CreerAnomalieAsync(Anomalie anomalie);
        
        // Logique métier : Changement de statut et fermeture automatique
        Task ChangerStatutAsync(int anomalieId, int nouveauStatutId);
        
        // Logique métier : Assignation d'un technicien
        Task AssignerTechnicienAsync(int anomalieId, int technicienId);
    }
}
