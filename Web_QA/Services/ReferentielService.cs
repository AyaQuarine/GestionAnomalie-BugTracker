using GestionAnomalies.Entities;
using GestionAnomalies.Repositories.Interfaces;
using GestionAnomalies.Services.Interfaces;

namespace GestionAnomalies.Services
{
    public class ReferentielService : IReferentielService
    {
        private readonly ITypeAnomalieRepository _typeRepo;
        private readonly IPrioriteRepository _prioriteRepo;
        private readonly IStatutRepository _statutRepo;
        private readonly IUserRepository _userRepo;

        public ReferentielService(
            ITypeAnomalieRepository typeRepo,
            IPrioriteRepository prioriteRepo,
            IStatutRepository statutRepo,
            IUserRepository userRepo)
        {
            _typeRepo = typeRepo;
            _prioriteRepo = prioriteRepo;
            _statutRepo = statutRepo;
            _userRepo = userRepo;
        }

        public async Task<IEnumerable<TypeAnomalie>> GetTypesAsync() => await _typeRepo.GetAllAsync();
        public async Task<IEnumerable<Priorite>> GetPrioritesAsync() => await _prioriteRepo.GetAllAsync();
        public async Task<IEnumerable<Statut>> GetStatutsAsync() => await _statutRepo.GetAllAsync();
        public async Task<IEnumerable<User>> GetTechniciensAsync() => await _userRepo.GetTechniciensAsync();
        
        public async Task<IEnumerable<User>> GetResponsablesAsync()
        {
            var allUsers = await _userRepo.GetAllAsync();
            return allUsers.Where(u => u.Role == Entities.Enums.RoleUser.Responsable || u.Role == Entities.Enums.RoleUser.Administrateur);
        }
    }
}
