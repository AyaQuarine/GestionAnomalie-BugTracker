using System.ComponentModel.DataAnnotations;

namespace GestionAnomalies.Entities
{
    public class TypeAnomalie : Base
    {
        [Required]
        [StringLength(50)]
        [Display(Name = "Libellé")]
        public string Libelle { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; } = string.Empty;

        // --- RELATIONS DE NAVIGATION ---
        
        // Les anomalies de ce type
        public virtual ICollection<Anomalie> Anomalies { get; set; } = new List<Anomalie>();
    }
}
