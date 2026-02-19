using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionAnomalies.Entities
{
    public class Commentaire : Base
    {
        [Required]
        [DataType(DataType.MultilineText)]
        public string Message { get; set; } = string.Empty;

        // --- CLES ETRANGERES ---

        // Anomalie associée
        public int AnomalieId { get; set; }
        [ForeignKey("AnomalieId")]
        public virtual Anomalie? Anomalie { get; set; }

        // Auteur du commentaire (User)
        public int AuteurId { get; set; }
        [ForeignKey("AuteurId")]
        public virtual User? Auteur { get; set; }
    }
}
