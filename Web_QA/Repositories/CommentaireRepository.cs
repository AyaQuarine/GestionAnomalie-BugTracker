using Microsoft.EntityFrameworkCore;
using GestionAnomalies.Data;
using GestionAnomalies.Entities;
using GestionAnomalies.Repositories.Interfaces;

namespace GestionAnomalies.Repositories
{
    public class CommentaireRepository : ICommentaireRepository
    {
        private readonly AppDbContext _context;

        public CommentaireRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Commentaire>> GetByAnomalieIdAsync(int anomalieId)
        {
            return await _context.Commentaires
                .Include(c => c.Auteur)
                .Where(c => c.AnomalieId == anomalieId)
                .OrderBy(c => c.DateCreation)
                .ToListAsync();
        }

        public async Task AddAsync(Commentaire commentaire)
        {
            await _context.Commentaires.AddAsync(commentaire);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var com = await _context.Commentaires.FindAsync(id);
            if (com != null)
            {
                _context.Commentaires.Remove(com);
                await _context.SaveChangesAsync();
            }
        }
    }
}
