using GestionAnomalies.Entities;

namespace GestionAnomalies.Models
{
    public class ProjetDetailsViewModel
    {
        public Projet Projet { get; set; } = null!;

        // Statistiques du projet
        public int NombreAnomalies { get; set; }
        public int NombreAnomaliesOuvertes { get; set; }
        public int NombreAnomaliesFermees { get; set; }
        public int NombreAnomaliesCritiques { get; set; }

        // Anomalies du projet
        public IEnumerable<Anomalie> Anomalies { get; set; } = new List<Anomalie>();

        // Permissions
        public bool PeutModifier { get; set; }
        public bool PeutSupprimer { get; set; }
    }
}
