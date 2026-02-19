using System.ComponentModel.DataAnnotations;
using GestionAnomalies.Entities;
using Microsoft.AspNetCore.Http;

namespace GestionAnomalies.Models
{
    public class AnomalieDetailsViewModel
    {
        public Anomalie Anomalie { get; set; } = null!;

        // Pour changer le statut
        [Display(Name = "Nouveau statut")]
        public int? NouveauStatutId { get; set; }
        public IEnumerable<Statut> StatutsDisponibles { get; set; } = new List<Statut>();

        // Pour assigner un technicien
        [Display(Name = "Technicien")]
        public int? TechnicienId { get; set; }
        public IEnumerable<User> TechniciensDisponibles { get; set; } = new List<User>();

        // Commentaires
        public IEnumerable<Commentaire> Commentaires { get; set; } = new List<Commentaire>();
        
        [Display(Name = "Nouveau commentaire")]
        public string? NouveauCommentaire { get; set; }

        // Pièces jointes
        public IEnumerable<PieceJointe> PiecesJointes { get; set; } = new List<PieceJointe>();
        
        [Display(Name = "Fichier à joindre")]
        public IFormFile? FichierAJoindre { get; set; }

        // Permissions utilisateur
        public bool PeutModifier { get; set; }
        public bool PeutSupprimer { get; set; }
        public bool PeutChangerStatut { get; set; }
        public bool PeutAssignerTechnicien { get; set; }
    }
}
