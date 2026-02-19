using GestionAnomalies.Entities;

namespace GestionAnomalies.Services.Interfaces
{
    public interface IProjetService
    {
        Task<IEnumerable<Projet>> GetAllProjetsAsync();
        Task CreerProjetAsync(Projet projet);
        Task ModifierProjetAsync(Projet projet);
    }
}
