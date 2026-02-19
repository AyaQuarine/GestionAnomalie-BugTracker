using GestionAnomalies.Entities;

namespace GestionAnomalies.Repositories.Interfaces
{
    public interface ICommentaireRepository
    {
        Task<IEnumerable<Commentaire>> GetByAnomalieIdAsync(int anomalieId);
        Task AddAsync(Commentaire commentaire);
        Task DeleteAsync(int id);
    }
}
