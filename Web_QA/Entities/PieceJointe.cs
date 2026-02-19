using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionAnomalies.Entities
{
    public class PieceJointe : Base
    {
        [Required]
        [StringLength(255)]
        [Display(Name = "Nom du fichier")]
        public string NomFichier { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Chemin du fichier")]
        public string CheminFichier { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Taille (octets)")]
        public long Taille { get; set; }

        // --- CLES ETRANGERES ---

        // Anomalie associée
        public int AnomalieId { get; set; }
        [ForeignKey("AnomalieId")]
        public virtual Anomalie? Anomalie { get; set; }
    }
}
