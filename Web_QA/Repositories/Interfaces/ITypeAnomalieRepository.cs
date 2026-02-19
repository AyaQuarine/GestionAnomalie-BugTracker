using GestionAnomalies.Entities;

namespace GestionAnomalies.Repositories.Interfaces
{
    public interface ITypeAnomalieRepository
    {
        Task<IEnumerable<TypeAnomalie>> GetAllAsync();
        Task<TypeAnomalie?> GetByIdAsync(int id);
        Task<TypeAnomalie> AddAsync(TypeAnomalie type);
        Task<TypeAnomalie> UpdateAsync(TypeAnomalie type);
        Task DeleteAsync(int id);
    }
}
