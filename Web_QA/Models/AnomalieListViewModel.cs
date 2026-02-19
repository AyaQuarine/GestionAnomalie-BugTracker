using System.ComponentModel.DataAnnotations;
using GestionAnomalies.Entities;

namespace GestionAnomalies.Models
{
    public class AnomalieListViewModel
    {
        public IEnumerable<Anomalie> Anomalies { get; set; } = new List<Anomalie>();

        // Filtres
        public int? ProjetIdFiltre { get; set; }
        public int? StatutIdFiltre { get; set; }
        public int? PrioriteIdFiltre { get; set; }
        public int? TechnicienIdFiltre { get; set; }
        public string? MotCleFiltre { get; set; }

        // Listes déroulantes pour les filtres
        public IEnumerable<Projet> Projets { get; set; } = new List<Projet>();
        public IEnumerable<Statut> Statuts { get; set; } = new List<Statut>();
        public IEnumerable<Priorite> Priorites { get; set; } = new List<Priorite>();
        public IEnumerable<User> Techniciens { get; set; } = new List<User>();
    }
}
