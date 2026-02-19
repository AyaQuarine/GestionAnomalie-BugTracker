using GestionAnomalies.Entities;

namespace GestionAnomalies.Repositories.Interfaces
{
    public interface IAnomalieRepository
    {
        // Contrat : Voici ce qu'on peut faire avec les anomalies
        //IEnumerable c est pour 
        Task<IEnumerable<Anomalie>> GetAllAsync();
        Task<IEnumerable<Anomalie>> GetByProjetIdsAsync(IEnumerable<int> projetIds); //pour responsable
        Task<IEnumerable<Anomalie>> GetByTechnicienAsync(int userId);
        Task<IEnumerable<Anomalie>> GetByCreateurAsync(int userId);
        Task<Anomalie?> GetByIdAsync(int id);
        Task AddAsync(Anomalie anomalie);
        Task UpdateAsync(Anomalie anomalie);
        Task DeleteAsync(int id);
    }
}