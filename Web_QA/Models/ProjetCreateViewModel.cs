using System.ComponentModel.DataAnnotations;
using GestionAnomalies.Entities;

namespace GestionAnomalies.Models
{
    public class ProjetCreateViewModel
    {
        [Required(ErrorMessage = "Le nom est obligatoire")]
        [StringLength(100, ErrorMessage = "Le nom ne peut pas dépasser 100 caractères")]
        [Display(Name = "Nom du projet")]
        public string Nom { get; set; } = string.Empty;

        [Display(Name = "Description")]
        public string? Description { get; set; }

        [Display(Name = "Responsable")]
        public int? ResponsableId { get; set; }

        // Liste déroulante
        public IEnumerable<User> ResponsablesDisponibles { get; set; } = new List<User>();
    }
}
