using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace GestionAnomalies.Models
{
    public class PieceJointeUploadViewModel
    {
        public int AnomalieId { get; set; }

        [Required(ErrorMessage = "Veuillez sélectionner un fichier")]
        [Display(Name = "Fichier")]
        public IFormFile Fichier { get; set; } = null!;
    }
}
