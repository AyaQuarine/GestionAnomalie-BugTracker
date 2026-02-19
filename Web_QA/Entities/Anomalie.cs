using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionAnomalies.Entities
{
    public class Anomalie : Base
    {
        [Required]
        [StringLength(100)]
        public string Titre { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        [Display(Name = "Date de clôture")]
        public DateTime? DateCloture { get; set; }
        
        // --- CLES ETRANGERES ---

        // Créateur (User)
        public int CreateurId { get; set; }
        [ForeignKey("CreateurId")]
        public virtual User? Createur { get; set; }

        // Technicien Assigné (User) - Peut être null au début
        public int? AssigneeId { get; set; }
        [ForeignKey("AssigneeId")]
        public virtual User? Assignee { get; set; }

        // Projet associé (obligatoire)
        [Required]
        public int ProjetId { get; set; }
        [ForeignKey("ProjetId")]
        public virtual Projet? Projet { get; set; }

        // Type d'anomalie (obligatoire)
        [Required]
        public int TypeAnomalieId { get; set; }
        [ForeignKey("TypeAnomalieId")]
        public virtual TypeAnomalie? TypeAnomalie { get; set; }

        // Priorité (obligatoire)
        [Required]
        public int PrioriteId { get; set; }
        [ForeignKey("PrioriteId")]
        public virtual Priorite? Priorite { get; set; }

        // Statut (obligatoire)
        [Required]
        public int StatutId { get; set; }
        [ForeignKey("StatutId")]
        public virtual Statut? Statut { get; set; }

        // Commentaires sur cette anomalie
        public virtual ICollection<Commentaire> Commentaires { get; set; } = new List<Commentaire>();

        // Pièces jointes
        public virtual ICollection<PieceJointe> PiecesJointes { get; set; } = new List<PieceJointe>();
    }
}
