using System.ComponentModel.DataAnnotations;

namespace GestionAnomalies.Entities
{
    public class Priorite : Base
    {
        [Required]
        [StringLength(50)]
        [Display(Name = "Libellé")]
        public string Libelle { get; set; } = string.Empty; // Critique, Majeure, Mineure, etc.

        [Required]
        [StringLength(7)] // Format hex couleur #RRGGBB
        [Display(Name = "Code couleur")]
        public string CodeCouleur { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Niveau de priorité")]
        public int Niveau { get; set; } // 1 = plus basse, 5 = plus haute

        // --- RELATIONS DE NAVIGATION ---
        
        // Les anomalies avec cette priorité
        public virtual ICollection<Anomalie> Anomalies { get; set; } = new List<Anomalie>();
    }
}
