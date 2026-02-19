using Microsoft.EntityFrameworkCore;
using GestionAnomalies.Data;
using GestionAnomalies.Entities;
using GestionAnomalies.Repositories.Interfaces;

namespace GestionAnomalies.Repositories
{
    public class PrioriteRepository : IPrioriteRepository
    {
        private readonly AppDbContext _context;

        public PrioriteRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Priorite>> GetAllAsync()
        {
            // On trie par niveau (Haute > Faible)
            return await _context.Priorites.OrderByDescending(p => p.Niveau).ToListAsync();
        }

        public async Task<Priorite?> GetByIdAsync(int id)
        {
            return await _context.Priorites.FindAsync(id);
        }

        public async Task<Priorite> AddAsync(Priorite priorite)
        {
            _context.Priorites.Add(priorite);
            await _context.SaveChangesAsync();
            return priorite;
        }

        public async Task<Priorite> UpdateAsync(Priorite priorite)
        {
            _context.Priorites.Update(priorite);
            await _context.SaveChangesAsync();
            return priorite;
        }

        public async Task DeleteAsync(int id)
        {
            var priorite = await GetByIdAsync(id);
            if (priorite != null)
            {
                _context.Priorites.Remove(priorite);
                await _context.SaveChangesAsync();
            }
        }
    }
}
