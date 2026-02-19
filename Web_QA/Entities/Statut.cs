using System.ComponentModel.DataAnnotations;

namespace GestionAnomalies.Entities

{
    public class Statut : Base
    {
        [Required]
        [StringLength(50)]
        [Display(Name = "Libellé")]
        public string Libelle { get; set; } = string.Empty; // Nouvelle, En cours, Résolue, Fermée, etc.

        [Required]
        [Display(Name = "État final")]
        public bool EstFinal { get; set; } // true si c'est un état terminal (Fermée, Rejetée, etc.)

        // --- RELATIONS DE NAVIGATION ---
        
        // Les anomalies avec ce statut
        public virtual ICollection<Anomalie> Anomalies { get; set; } = new List<Anomalie>();
    }
}
