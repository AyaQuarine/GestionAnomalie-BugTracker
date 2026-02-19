using GestionAnomalies.Entities;

namespace GestionAnomalies.Models
{
    public class DashboardViewModel
    {
        // Statistiques globales
        public int TotalAnomalies { get; set; }
        public int AnomaliesOuvertes { get; set; }
        public int AnomaliesEnCours { get; set; }
        public int AnomaliesFermees { get; set; }
        
        public int AnomaliesCritiques { get; set; }
        public int AnomaliesHautes { get; set; }
        public int AnomaliesMoyennes { get; set; }
        public int AnomaliesBasses { get; set; }

        // Statistiques par rôle
        public int MesAnomalies { get; set; }
        public int MesProjets { get; set; }

        // Anomalies récentes
        public IEnumerable<Anomalie> AnomaliesRecentes { get; set; } = new List<Anomalie>();

        // Anomalies non assignées (pour Admin/Responsable)
        public IEnumerable<Anomalie> AnomaliesNonAssignees { get; set; } = new List<Anomalie>();

        // Mes anomalies en tant que technicien
        public IEnumerable<Anomalie> MesAnomaliesAssignees { get; set; } = new List<Anomalie>();

        // Informations utilisateur
        public string NomUtilisateur { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;

        // Nouvelles statistiques pour le dashboard
        // Nombre d'anomalies par statut
        public Dictionary<string, int> AnomaliesParStatut { get; set; } = new Dictionary<string, int>();

        // Nombre d'anomalies par technicien
        public Dictionary<string, int> AnomaliesParTechnicien { get; set; } = new Dictionary<string, int>();

        // Nombre d'anomalies par priorité
        public Dictionary<string, int> AnomaliesParPriorite { get; set; } = new Dictionary<string, int>();

        // Temps moyen de résolution (en jours)
        public double TempsMoyenResolution { get; set; }
    }
}
