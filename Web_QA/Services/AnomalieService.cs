using GestionAnomalies.Entities;
using GestionAnomalies.Entities.Enums;
using GestionAnomalies.Repositories.Interfaces;
using GestionAnomalies.Services.Interfaces;

namespace GestionAnomalies.Services
{
    public class AnomalieService : IAnomalieService
    {
        private readonly IAnomalieRepository _anomalieRepo;
        private readonly IStatutRepository _statutRepo;
        private readonly IProjetRepository _projetRepo;

        public AnomalieService(IAnomalieRepository anomalieRepo, IStatutRepository statutRepo, IProjetRepository projetRepo)
        {
            _anomalieRepo = anomalieRepo;
            _statutRepo = statutRepo;
            _projetRepo = projetRepo;
        }

        public async Task<IEnumerable<Anomalie>> GetAnomaliesPourUtilisateurAsync(int userId, string role)
        {
            // RÈGLE MÉTIER : Filtrage selon le rôle
            
            // 1. Admin voit TOUT
            if (role == RoleUser.Administrateur.ToString())
            {
                return await _anomalieRepo.GetAllAsync();
            }
            
            // 2. Responsable voit les anomalies de SES projets uniquement
            if (role == RoleUser.Responsable.ToString())
            {
                var mesProjets = await _projetRepo.GetByResponsableIdAsync(userId);
                var projetIds = mesProjets.Select(p => p.Id).ToList();
                
                if (!projetIds.Any())
                {
                    return new List<Anomalie>(); // Aucun projet affecté
                }
                
                return await _anomalieRepo.GetByProjetIdsAsync(projetIds);
            }
            
            // 3. Technicien voit ses assignations uniquement
            if (role == RoleUser.Technicien.ToString())
            {
                return await _anomalieRepo.GetByTechnicienAsync(userId);
            }

            // 4. Utilisateur voit ses déclarations uniquement
            return await _anomalieRepo.GetByCreateurAsync(userId);
        }

        public async Task<Anomalie?> GetDetailsAsync(int id)
        {
            return await _anomalieRepo.GetByIdAsync(id);
        }

        public async Task CreerAnomalieAsync(Anomalie anomalie)
        {
            // RÈGLE MÉTIER : Une nouvelle anomalie a toujours le statut "Nouvelle"
            var statuts = await _statutRepo.GetAllAsync();
            var statutNouvelle = statuts.FirstOrDefault(s => s.Libelle == "Nouvelle");

            if (statutNouvelle != null)
            {
                anomalie.StatutId = statutNouvelle.Id;
            }

            anomalie.DateCreation = DateTime.Now;
            await _anomalieRepo.AddAsync(anomalie);
        }

        public async Task ChangerStatutAsync(int anomalieId, int nouveauStatutId)
        {
            var anomalie = await _anomalieRepo.GetByIdAsync(anomalieId);
            if (anomalie == null) return;

            var nouveauStatut = await _statutRepo.GetByIdAsync(nouveauStatutId);
            
            // Mise à jour du statut
            anomalie.StatutId = nouveauStatutId;
            anomalie.DateModification = DateTime.Now;

            // RÈGLE MÉTIER : Si le statut est final (ex: Résolue), on met la date de clôture
            if (nouveauStatut != null && nouveauStatut.EstFinal)
            {
                anomalie.DateCloture = DateTime.Now;
            }
            else
            {
                // Si on réouvre le ticket, on efface la date de clôture
                anomalie.DateCloture = null;
            }

            await _anomalieRepo.UpdateAsync(anomalie);
        }

        public async Task AssignerTechnicienAsync(int anomalieId, int technicienId)
        {
            var anomalie = await _anomalieRepo.GetByIdAsync(anomalieId);
            if (anomalie != null)
            {
                anomalie.AssigneeId = technicienId;
                anomalie.DateModification = DateTime.Now;
                
                // Optionnel : On peut passer le statut à "En cours" automatiquement
                await _anomalieRepo.UpdateAsync(anomalie);
            }
        }
    }
}
