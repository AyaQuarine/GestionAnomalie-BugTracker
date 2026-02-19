using GestionAnomalies.Entities;

namespace GestionAnomalies.Services.Interfaces
{
    public interface IReferentielService
    {
        Task<IEnumerable<TypeAnomalie>> GetTypesAsync();
        Task<IEnumerable<Priorite>> GetPrioritesAsync();
        Task<IEnumerable<Statut>> GetStatutsAsync();
        Task<IEnumerable<User>> GetTechniciensAsync();
        Task<IEnumerable<User>> GetResponsablesAsync();
    }
}
