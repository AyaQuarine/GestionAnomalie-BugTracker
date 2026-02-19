using GestionAnomalies.Entities;

namespace GestionAnomalies.Repositories.Interfaces
{
    public interface IProjetRepository
    {
        Task<IEnumerable<Projet>> GetAllAsync();
        Task<IEnumerable<Projet>> GetByResponsableIdAsync(int responsableId); // Pour Responsable
        Task<Projet?> GetByIdAsync(int id);
        Task AddAsync(Projet projet);
        Task UpdateAsync(Projet projet);
        Task DeleteAsync(int id);
    }
}
