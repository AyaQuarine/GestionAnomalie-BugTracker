using GestionAnomalies.Entities;

namespace GestionAnomalies.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByIdAsync(int id);
        Task AddAsync(User user);
        Task<User> UpdateAsync(User user);
        Task DeleteAsync(int id);
        
        // Utile pour la liste des techniciens (dropdowns)
        Task<IEnumerable<User>> GetTechniciensAsync(); 
        
        // Utile pour la gestion admin
        Task<IEnumerable<User>> GetAllAsync();
    }
}