using Microsoft.EntityFrameworkCore;
using GestionAnomalies.Data;
using GestionAnomalies.Entities;
using GestionAnomalies.Repositories.Interfaces;

namespace GestionAnomalies.Repositories
{
    public class ProjetRepository : IProjetRepository
    {
        private readonly AppDbContext _context;

        public ProjetRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Projet>> GetAllAsync()
        {
            return await _context.Projets
                .Include(p => p.Anomalies) 
                .OrderByDescending(p => p.DateDebut)
                .ToListAsync();
        }

        public async Task<IEnumerable<Projet>> GetByResponsableIdAsync(int responsableId)
        {
            return await _context.Projets
                .Include(p => p.Anomalies)
                .Where(p => p.ResponsableId == responsableId)
                .OrderByDescending(p => p.DateDebut)
                .ToListAsync();
        }

        public async Task<Projet?> GetByIdAsync(int id)
        {
            return await _context.Projets
                .Include(p => p.Anomalies)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task AddAsync(Projet projet)
        {
            await _context.Projets.AddAsync(projet);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Projet projet)
        {
            _context.Projets.Update(projet);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var projet = await _context.Projets.FindAsync(id);
            if (projet != null)
            {
                _context.Projets.Remove(projet);
                await _context.SaveChangesAsync();
            }
        }
    }
}
