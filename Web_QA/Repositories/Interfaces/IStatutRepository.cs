using GestionAnomalies.Entities;

namespace GestionAnomalies.Repositories.Interfaces
{
    public interface IStatutRepository
    {
        Task<IEnumerable<Statut>> GetAllAsync();
        Task<Statut?> GetByIdAsync(int id);
        Task<Statut> AddAsync(Statut statut);
        Task<Statut> UpdateAsync(Statut statut);
        Task DeleteAsync(int id);
    }
}
