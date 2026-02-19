using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionAnomalies.Entities
{
    public class Projet : Base
    {
        [Required]
        [StringLength(100)]
        public string Nom { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Date de début")]
        public DateTime DateDebut { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Date de fin")]
        public DateTime DateFin { get; set; }

        // --- AFFECTATION AU RESPONSABLE ---
        [Display(Name = "Responsable")]
        public int? ResponsableId { get; set; } // Nullable car Admin peut créer sans affecter

        [ForeignKey(nameof(ResponsableId))]
        public virtual User? Responsable { get; set; }

        // --- RELATIONS DE NAVIGATION ---
        
        // Les anomalies associées à ce projet
        public virtual ICollection<Anomalie> Anomalies { get; set; } = new List<Anomalie>();
    }
}
