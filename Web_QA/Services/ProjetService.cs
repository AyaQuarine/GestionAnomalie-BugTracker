using GestionAnomalies.Entities;
using GestionAnomalies.Repositories.Interfaces;
using GestionAnomalies.Services.Interfaces;

namespace GestionAnomalies.Services
{
    public class ProjetService : IProjetService
    {
        private readonly IProjetRepository _projetRepo;

        public ProjetService(IProjetRepository projetRepo)
        {
            _projetRepo = projetRepo;
        }

        public async Task<IEnumerable<Projet>> GetAllProjetsAsync()
        {
            return await _projetRepo.GetAllAsync();
        }

        public async Task CreerProjetAsync(Projet projet)
        {
            // Règle métier : Vérifier date fin > date début
            if (projet.DateFin < projet.DateDebut)
            {
                throw new ArgumentException("La date de fin doit être après la date de début.");
            }
            
            projet.DateCreation = DateTime.Now;
            await _projetRepo.AddAsync(projet);
        }

        public async Task ModifierProjetAsync(Projet projet)
        {
            // Règle métier : Vérifier date fin > date début
            if (projet.DateFin < projet.DateDebut)
            {
                throw new ArgumentException("La date de fin doit être après la date de début.");
            }
            
            projet.DateModification = DateTime.Now;
            await _projetRepo.UpdateAsync(projet);
        }
    }
}
