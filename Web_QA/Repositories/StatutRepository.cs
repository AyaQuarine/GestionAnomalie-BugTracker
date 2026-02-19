using Microsoft.EntityFrameworkCore;
using GestionAnomalies.Data;
using GestionAnomalies.Entities;
using GestionAnomalies.Repositories.Interfaces;

namespace GestionAnomalies.Repositories
{
    public class StatutRepository : IStatutRepository
    {
        private readonly AppDbContext _context;

        public StatutRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Statut>> GetAllAsync()
        {
            return await _context.Statuts.ToListAsync();
        }

        public async Task<Statut?> GetByIdAsync(int id)
        {
            return await _context.Statuts.FindAsync(id);
        }

        public async Task<Statut> AddAsync(Statut statut)
        {
            _context.Statuts.Add(statut);
            await _context.SaveChangesAsync();
            return statut;
        }

        public async Task<Statut> UpdateAsync(Statut statut)
        {
            _context.Statuts.Update(statut);
            await _context.SaveChangesAsync();
            return statut;
        }

        public async Task DeleteAsync(int id)
        {
            var statut = await GetByIdAsync(id);
            if (statut != null)
            {
                _context.Statuts.Remove(statut);
                await _context.SaveChangesAsync();
            }
        }
    }
}
