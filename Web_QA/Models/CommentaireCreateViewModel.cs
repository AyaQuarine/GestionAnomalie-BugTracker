using System.ComponentModel.DataAnnotations;

namespace GestionAnomalies.Models
{
    public class CommentaireCreateViewModel
    {
        public int AnomalieId { get; set; }

        [Required(ErrorMessage = "Le contenu du commentaire est obligatoire")]
        [Display(Name = "Commentaire")]
        [StringLength(1000, ErrorMessage = "Le commentaire ne peut pas dépasser 1000 caractères")]
        public string Contenu { get; set; } = string.Empty;
    }
}
