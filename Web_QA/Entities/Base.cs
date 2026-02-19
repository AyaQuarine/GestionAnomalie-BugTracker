using System.ComponentModel.DataAnnotations;

namespace GestionAnomalies.Entities
{
    public abstract class Base
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Date de création")]
        public DateTime DateCreation { get; set; } = DateTime.Now;

        [Display(Name = "Dernière modification")]
        public DateTime? DateModification { get; set; }
    }
}
