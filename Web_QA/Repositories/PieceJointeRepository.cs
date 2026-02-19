using Microsoft.EntityFrameworkCore;
using GestionAnomalies.Data;
using GestionAnomalies.Entities;
using GestionAnomalies.Repositories.Interfaces;

namespace GestionAnomalies.Repositories
{
    public class PieceJointeRepository : IPieceJointeRepository
    {
        private readonly AppDbContext _context;

        public PieceJointeRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PieceJointe>> GetByAnomalieIdAsync(int anomalieId)
        {
            return await _context.PiecesJointes
                .Where(p => p.AnomalieId == anomalieId)
                .OrderBy(p => p.DateCreation)
                .ToListAsync();
        }

        public async Task AddAsync(PieceJointe pieceJointe)
        {
            await _context.PiecesJointes.AddAsync(pieceJointe);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var pj = await _context.PiecesJointes.FindAsync(id);
            if (pj != null)
            {
                _context.PiecesJointes.Remove(pj);
                await _context.SaveChangesAsync();
            }
        }
    }
}
