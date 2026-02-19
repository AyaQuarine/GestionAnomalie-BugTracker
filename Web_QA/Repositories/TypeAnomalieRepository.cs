using Microsoft.EntityFrameworkCore;
using GestionAnomalies.Data;
using GestionAnomalies.Entities;
using GestionAnomalies.Repositories.Interfaces;

namespace GestionAnomalies.Repositories
{
    public class TypeAnomalieRepository : ITypeAnomalieRepository
    {
        private readonly AppDbContext _context;

        public TypeAnomalieRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TypeAnomalie>> GetAllAsync()
        {
            return await _context.TypesAnomalies.ToListAsync();
        }

        public async Task<TypeAnomalie?> GetByIdAsync(int id)
        {
            return await _context.TypesAnomalies.FindAsync(id);
        }

        public async Task<TypeAnomalie> AddAsync(TypeAnomalie type)
        {
            _context.TypesAnomalies.Add(type);
            await _context.SaveChangesAsync();
            return type;
        }

        public async Task<TypeAnomalie> UpdateAsync(TypeAnomalie type)
        {
            _context.TypesAnomalies.Update(type);
            await _context.SaveChangesAsync();
            return type;
        }

        public async Task DeleteAsync(int id)
        {
            var type = await GetByIdAsync(id);
            if (type != null)
            {
                _context.TypesAnomalies.Remove(type);
                await _context.SaveChangesAsync();
            }
        }
    }
}
