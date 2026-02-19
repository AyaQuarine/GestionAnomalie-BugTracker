using GestionAnomalies.Entities;

namespace GestionAnomalies.Repositories.Interfaces
{
    public interface IPieceJointeRepository
    {
        Task<IEnumerable<PieceJointe>> GetByAnomalieIdAsync(int anomalieId);
        Task AddAsync(PieceJointe pieceJointe);
        Task DeleteAsync(int id);
    }
}
