using System.ComponentModel.DataAnnotations;
using GestionAnomalies.Entities;

namespace GestionAnomalies.Models
{
    public class AnomalieCreateViewModel
    {
        [Required(ErrorMessage = "Le titre est obligatoire")]
        [StringLength(200, ErrorMessage = "Le titre ne peut pas dépasser 200 caractères")]
        [Display(Name = "Titre")]
        public string Titre { get; set; } = string.Empty;

        [Required(ErrorMessage = "La description est obligatoire")]
        [Display(Name = "Description")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le projet est obligatoire")]
        [Display(Name = "Projet")]
        public int ProjetId { get; set; }

        [Required(ErrorMessage = "Le type d'anomalie est obligatoire")]
        [Display(Name = "Type d'anomalie")]
        public int TypeAnomalieId { get; set; }

        [Required(ErrorMessage = "La priorité est obligatoire")]
        [Display(Name = "Priorité")]
        public int PrioriteId { get; set; }

        // Listes déroulantes
        public IEnumerable<Projet> Projets { get; set; } = new List<Projet>();
        public IEnumerable<TypeAnomalie> TypesAnomalie { get; set; } = new List<TypeAnomalie>();
        public IEnumerable<Priorite> Priorites { get; set; } = new List<Priorite>();
    }
}
