using GestionAnomalies.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace GestionAnomalies.Entities
{
    public class User : Base
    {
        [Required(ErrorMessage = "Le nom est obligatoire")]
        [StringLength(50)]
        public string Nom { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le prénom est obligatoire")]
        [StringLength(50)]
        public string Prenom { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required, DataType(DataType.Password)]
        public string MotDePasse { get; set; } = string.Empty; // En production, on hash le mot de passe !

        [Required]
        public RoleUser Role { get; set; }

        // --- RELATIONS DE NAVIGATION ---
        
        // Anomalies que j'ai créées (Déclarant)
        public virtual ICollection<Anomalie> AnomaliesDeclarees { get; set; } = new List<Anomalie>();

        // Anomalies qu'on m'a assignées (Technicien)
        public virtual ICollection<Anomalie> AnomaliesAssignees { get; set; } = new List<Anomalie>();

        // Commentaires que j'ai écrits
        public virtual ICollection<Commentaire> Commentaires { get; set; } = new List<Commentaire>();
    }
}
