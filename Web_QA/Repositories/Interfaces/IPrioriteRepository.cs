using GestionAnomalies.Entities;

namespace GestionAnomalies.Repositories.Interfaces
{
    public interface IPrioriteRepository
    {
        Task<IEnumerable<Priorite>> GetAllAsync();
        Task<Priorite?> GetByIdAsync(int id);
        Task<Priorite> AddAsync(Priorite priorite);
        Task<Priorite> UpdateAsync(Priorite priorite);
        Task DeleteAsync(int id);
    }
}
