using Microsoft.EntityFrameworkCore;
using GestionAnomalies.Data;
using GestionAnomalies.Entities;
using GestionAnomalies.Repositories.Interfaces;

namespace GestionAnomalies.Repositories
{
    public class AnomalieRepository : IAnomalieRepository
    {
        private readonly AppDbContext _context;

        public AnomalieRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Anomalie>> GetAllAsync()
        {
            return await _context.Anomalies
                .Include(a => a.Statut)
                .Include(a => a.Priorite)
                .Include(a => a.TypeAnomalie)
                .Include(a => a.Createur)
                .Include(a => a.Assignee)
                .OrderByDescending(a => a.DateCreation)
                .ToListAsync();
        }

        public async Task<IEnumerable<Anomalie>> GetByProjetIdsAsync(IEnumerable<int> projetIds)
        {
            return await _context.Anomalies
                .Include(a => a.Statut)
                .Include(a => a.Priorite)
                .Include(a => a.TypeAnomalie)
                .Include(a => a.Createur)
                .Include(a => a.Assignee)
                .Where(a => projetIds.Contains(a.ProjetId))
                .OrderByDescending(a => a.DateCreation)
                .ToListAsync();
        }

        public async Task<IEnumerable<Anomalie>> GetByTechnicienAsync(int userId)
        {
            return await _context.Anomalies
                .Include(a => a.Statut)
                .Include(a => a.Priorite)
                .Where(a => a.AssigneeId == userId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Anomalie>> GetByCreateurAsync(int userId)
        {
            return await _context.Anomalies
                .Include(a => a.Statut)
                .Include(a => a.Priorite)
                .Where(a => a.CreateurId == userId)
                .ToListAsync();
        }

        public async Task<Anomalie?> GetByIdAsync(int id)
        {
            return await _context.Anomalies
                .Include(a => a.Statut)
                .Include(a => a.Priorite)
                .Include(a => a.TypeAnomalie)
                .Include(a => a.Createur)
                .Include(a => a.Assignee)
                .Include(a => a.Projet)
                .Include(a => a.Commentaires)
                    .ThenInclude(c => c.Auteur)
                .Include(a => a.PiecesJointes)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task AddAsync(Anomalie anomalie)
        {
            // S'assurer que l'Id est 0 pour laisser SQL Server le générer automatiquement
            anomalie.Id = 0;
            await _context.Anomalies.AddAsync(anomalie);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Anomalie anomalie)
        {
            var existingAnomalie = await _context.Anomalies.FindAsync(anomalie.Id);
            
            if (existingAnomalie != null)
            {
                // Mise à jour uniquement des propriétés modifiables
                existingAnomalie.Titre = anomalie.Titre;
                existingAnomalie.Description = anomalie.Description;
                existingAnomalie.ProjetId = anomalie.ProjetId;
                existingAnomalie.TypeAnomalieId = anomalie.TypeAnomalieId;
                existingAnomalie.PrioriteId = anomalie.PrioriteId;
                existingAnomalie.StatutId = anomalie.StatutId;
                existingAnomalie.AssigneeId = anomalie.AssigneeId;
                existingAnomalie.DateModification = anomalie.DateModification;
                existingAnomalie.DateCloture = anomalie.DateCloture;
                
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(int id)
        {
            var anomalie = await _context.Anomalies.FindAsync(id);
            if (anomalie != null)
            {
                _context.Anomalies.Remove(anomalie);
                await _context.SaveChangesAsync();
            }
        }
    }
}