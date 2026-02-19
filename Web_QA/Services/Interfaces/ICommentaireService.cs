using GestionAnomalies.Entities;

namespace GestionAnomalies.Services.Interfaces
{
    public interface ICommentaireService
    {
        Task<IEnumerable<Commentaire>> GetByAnomalieIdAsync(int anomalieId);
        Task<bool> AddAsync(int anomalieId, int auteurId, string contenu);
        Task<bool> DeleteAsync(int id, int currentUserId);
    }
}
