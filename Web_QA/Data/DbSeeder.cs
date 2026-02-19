using GestionAnomalies.Entities;
using GestionAnomalies.Entities.Enums;
using Microsoft.EntityFrameworkCore;

namespace GestionAnomalies.Data
{
    public static class DbSeeder
    {
        public static void Seed(AppDbContext context)
        {
            try
            {
                // Appliquer les migrations si nécessaire
                context.Database.Migrate();

                // 1. STATUTS
            if (!context.Statuts.Any())
            {
                var statuts = new List<Statut>
                {
                    new Statut { Libelle = "Nouvelle", EstFinal = false, DateCreation = DateTime.Now },
                    new Statut { Libelle = "En cours", EstFinal = false, DateCreation = DateTime.Now },
                    new Statut { Libelle = "En attente", EstFinal = false, DateCreation = DateTime.Now },
                    new Statut { Libelle = "Résolue", EstFinal = true, DateCreation = DateTime.Now },
                    new Statut { Libelle = "Rejetée", EstFinal = true, DateCreation = DateTime.Now }
                };
                context.Statuts.AddRange(statuts);
                context.SaveChanges();
            }

            // 2. PRIORITÉS
            if (!context.Priorites.Any())
            {
                var priorites = new List<Priorite>
                {
                    new Priorite { Libelle = "Faible", CodeCouleur = "#28a745", Niveau = 1, DateCreation = DateTime.Now },
                    new Priorite { Libelle = "Moyenne", CodeCouleur = "#ffc107", Niveau = 2, DateCreation = DateTime.Now },
                    new Priorite { Libelle = "Haute", CodeCouleur = "#fd7e14", Niveau = 3, DateCreation = DateTime.Now },
                    new Priorite { Libelle = "Critique", CodeCouleur = "#dc3545", Niveau = 4, DateCreation = DateTime.Now }
                };
                context.Priorites.AddRange(priorites);
                context.SaveChanges();
            }

            // 3. TYPES D'ANOMALIES
            if (!context.TypesAnomalies.Any())
            {
                var types = new List<TypeAnomalie>
                {
                    new TypeAnomalie { Libelle = "Bug", Description = "Erreur logicielle", DateCreation = DateTime.Now },
                    new TypeAnomalie { Libelle = "Matériel", Description = "Problème physique", DateCreation = DateTime.Now },
                    new TypeAnomalie { Libelle = "Demande", Description = "Nouvelle fonctionnalité", DateCreation = DateTime.Now }
                };
                context.TypesAnomalies.AddRange(types);
                context.SaveChanges();
            }

            // 4. UTILISATEURS DE TEST
            if (!context.Users.Any())
            {
                var users = new List<User>
                {
                    new User
                    {
                        Nom = "Martinez",
                        Prenom = "Sarah",
                        Email = "admin@company.com",
                        MotDePasse = "123456",
                        Role = RoleUser.Administrateur,
                        DateCreation = DateTime.Now
                    },
                    new User
                    {
                        Nom = "Johnson",
                        Prenom = "Michael",
                        Email = "m.johnson@company.com",
                        MotDePasse = "123456",
                        Role = RoleUser.Responsable,
                        DateCreation = DateTime.Now
                    },
                    new User
                    {
                        Nom = "Chen",
                        Prenom = "David",
                        Email = "d.chen@company.com",
                        MotDePasse = "123456",
                        Role = RoleUser.Technicien,
                        DateCreation = DateTime.Now
                    },
                    new User
                    {
                        Nom = "Williams",
                        Prenom = "Emma",
                        Email = "e.williams@company.com",
                        MotDePasse = "123456",
                        Role = RoleUser.Technicien,
                        DateCreation = DateTime.Now
                    },
                    new User
                    {
                        Nom = "Brown",
                        Prenom = "Robert",
                        Email = "r.brown@company.com",
                        MotDePasse = "123456",
                        Role = RoleUser.Utilisateur,
                        DateCreation = DateTime.Now
                    }
                };
                context.Users.AddRange(users);
                context.SaveChanges();
            }

            // 5. PROJETS D'EXEMPLE
            if (!context.Projets.Any())
            {
                var responsable = context.Users.First(u => u.Email == "m.johnson@company.com");
                var admin = context.Users.First(u => u.Email == "admin@company.com");

                var projets = new List<Projet>
                {
                    new Projet
                    {
                        Nom = "Customer Portal V2",
                        Description = "Refonte complète du portail client avec une meilleure interface utilisateur.",
                        DateDebut = DateTime.Now.AddDays(-45),
                        DateFin = DateTime.Now.AddDays(75),
                        ResponsableId = responsable.Id,
                        DateCreation = DateTime.Now
                    },
                    new Projet
                    {
                        Nom = "Mobile Service App",
                        Description = "Application mobile pour les techniciens sur le terrain avec synchronisation hors ligne.",
                        DateDebut = DateTime.Now.AddDays(-20),
                        DateFin = DateTime.Now.AddDays(100),
                        ResponsableId = responsable.Id,
                        DateCreation = DateTime.Now
                    },
                    new Projet
                    {
                        Nom = "Analytics Dashboard",
                        Description = "Tableau de bord avec métriques en temps réel pour le suivi des projets.",
                        DateDebut = DateTime.Now.AddDays(-60),
                        DateFin = DateTime.Now.AddDays(45),
                        ResponsableId = admin.Id,
                        DateCreation = DateTime.Now
                    }
                };

                context.Projets.AddRange(projets);
                context.SaveChanges();
            }

            // 6. ANOMALIES D'EXEMPLE
            if (!context.Anomalies.Any())
            {
                var admin = context.Users.First(u => u.Email == "admin@company.com");
                var responsable = context.Users.First(u => u.Email == "m.johnson@company.com");
                var tech1 = context.Users.First(u => u.Email == "d.chen@company.com");
                var tech2 = context.Users.First(u => u.Email == "e.williams@company.com");

                var projetPortail = context.Projets.First(p => p.Nom == "Customer Portal V2");
                var projetMobile = context.Projets.First(p => p.Nom == "Mobile Service App");
                var projetAnalytics = context.Projets.First(p => p.Nom == "Analytics Dashboard");

                var typeBug = context.TypesAnomalies.First(t => t.Libelle == "Bug");
                var typeDemande = context.TypesAnomalies.First(t => t.Libelle == "Demande");
                var prioriteHaute = context.Priorites.First(p => p.Libelle == "Haute");
                var prioriteCritique = context.Priorites.First(p => p.Libelle == "Critique");
                var prioriteMoyenne = context.Priorites.First(p => p.Libelle == "Moyenne");
                var statutNouveau = context.Statuts.First(s => s.Libelle == "Nouvelle");
                var statutEnCours = context.Statuts.First(s => s.Libelle == "En cours");
                var statutAttente = context.Statuts.First(s => s.Libelle == "En attente");

                var anomalies = new List<Anomalie>
                {
                    new Anomalie
                    {
                        Titre = "Erreur 500 lors de la connexion après inactivité",
                        Description = "Les utilisateurs reçoivent une erreur 500 quand ils essaient de se connecter après avoir laissé la page ouverte sans rien faire pendant plus de 30 secondes. Le problème semble lié à l'expiration de la session.\n\nÉtapes pour reproduire :\n1. Aller sur la page de connexion\n2. Attendre 30+ secondes\n3. Entrer les identifiants et valider\n4. Erreur 500\n\nEnvironnement : Production, Chrome, Windows",
                        CreateurId = admin.Id,
                        AssigneeId = tech1.Id,
                        ProjetId = projetPortail.Id,
                        TypeAnomalieId = typeBug.Id,
                        PrioriteId = prioriteCritique.Id,
                        StatutId = statutEnCours.Id,
                        DateCreation = DateTime.Now.AddDays(-3)
                    },
                    new Anomalie
                    {
                        Titre = "Ajouter des filtres avancés pour la liste des anomalies",
                        Description = "En tant que chef de projet, je voudrais pouvoir filtrer les anomalies par plusieurs critères en même temps pour mieux suivre l'avancement.\n\nFonctionnalités souhaitées :\n- Filtrer par statut (sélection multiple)\n- Filtrer par priorité\n- Filtrer par technicien assigné\n- Filtrer par dates\n- Sauvegarder les filtres personnalisés\n\nCela nous ferait gagner beaucoup de temps pour les rapports.",
                        CreateurId = responsable.Id,
                        AssigneeId = tech2.Id,
                        ProjetId = projetPortail.Id,
                        TypeAnomalieId = typeDemande.Id,
                        PrioriteId = prioriteHaute.Id,
                        StatutId = statutNouveau.Id,
                        DateCreation = DateTime.Now.AddDays(-1)
                    },
                    new Anomalie
                    {
                        Titre = "L'appli mobile plante sur Android 14 avec les photos",
                        Description = "L'application mobile se ferme systématiquement quand on essaie d'attacher une photo depuis la galerie sur les téléphones Android 14. Ça bloque les techniciens qui ne peuvent pas documenter leurs interventions.\n\nDétails techniques :\n- Problème sur Android 14+ uniquement\n- Fonctionne sur Android 13 et versions antérieures\n- Plantage au moment de la sélection de fichier\n- Probablement lié aux nouvelles permissions de stockage\n\nSolution temporaire : prendre des photos directement avec l'appareil photo\n\nImpact : 40+ techniciens affectés quotidiennement",
                        CreateurId = responsable.Id,
                        AssigneeId = tech1.Id,
                        ProjetId = projetMobile.Id,
                        TypeAnomalieId = typeBug.Id,
                        PrioriteId = prioriteCritique.Id,
                        StatutId = statutEnCours.Id,
                        DateCreation = DateTime.Now.AddDays(-5)
                    },
                    new Anomalie
                    {
                        Titre = "Le tableau de bord est trop lent à charger",
                        Description = "Le tableau de bord des statistiques met 15-20 secondes à s'afficher quand il y a beaucoup de données. C'est gênant pendant les réunions.\n\nPerformances actuelles :\n- Chargement initial : 18 secondes en moyenne\n- Affichage des graphiques : 8 secondes\n- Actualisation : 12 secondes\n\nObjectif :\n- Chargement initial : moins de 3 secondes\n- Graphiques : moins de 2 secondes\n- Actualisation : moins de 5 secondes\n\nIdées d'optimisation :\n- Pagination des données\n- Cache côté serveur\n- Optimisation des requêtes SQL",
                        CreateurId = admin.Id,
                        AssigneeId = tech2.Id,
                        ProjetId = projetAnalytics.Id,
                        TypeAnomalieId = typeDemande.Id,
                        PrioriteId = prioriteMoyenne.Id,
                        StatutId = statutAttente.Id,
                        DateCreation = DateTime.Now.AddDays(-7)
                    }
                };

                context.Anomalies.AddRange(anomalies);
                context.SaveChanges();
            }

            // 7. COMMENTAIRES D'EXEMPLE
            if (!context.Commentaires.Any())
            {
                var admin = context.Users.First(u => u.Email == "admin@company.com");
                var tech1 = context.Users.First(u => u.Email == "d.chen@company.com");
                var tech2 = context.Users.First(u => u.Email == "e.williams@company.com");
                var responsable = context.Users.First(u => u.Email == "m.johnson@company.com");

                var anomalies = context.Anomalies.ToList();

                var comments = new List<Commentaire>
                {
                    new Commentaire
                    {
                        AnomalieId = anomalies[0].Id,
                        AuteurId = tech1.Id,
                        Message = "Enquête terminée. Le problème vient du mécanisme de renouvellement des tokens qui plante sous la charge. Je travaille sur un correctif avec une fenêtre d'expiration glissante.",
                        DateCreation = DateTime.Now.AddHours(-6)
                    },
                    new Commentaire
                    {
                        AnomalieId = anomalies[0].Id,
                        AuteurId = admin.Id,
                        Message = "Priorité critique confirmée. Merci de coordonner avec l'équipe DevOps pour le déploiement une fois le correctif prêt.",
                        DateCreation = DateTime.Now.AddHours(-4)
                    },
                    new Commentaire
                    {
                        AnomalieId = anomalies[1].Id,
                        AuteurId = tech2.Id,
                        Message = "Maquettes de l'interface terminées pour le composant de filtrage avancé. Je prévois d'utiliser React Select avec le style existant.",
                        DateCreation = DateTime.Now.AddHours(-12)
                    },
                    new Commentaire
                    {
                        AnomalieId = anomalies[1].Id,
                        AuteurId = responsable.Id,
                        Message = "Bon travail ! Pense à faire persister l'état du filtre au rafraîchissement de page. Les utilisateurs l'ont spécifiquement demandé.",
                        DateCreation = DateTime.Now.AddHours(-2)
                    },
                    new Commentaire
                    {
                        AnomalieId = anomalies[2].Id,
                        AuteurId = tech1.Id,
                        Message = "Reproduit sur Samsung Galaxy S24 (Android 14). Problème confirmé lié au nouveau modèle de permissions. Je travaille sur la mise à jour de compatibilité.",
                        DateCreation = DateTime.Now.AddHours(-8)
                    },
                    new Commentaire
                    {
                        AnomalieId = anomalies[3].Id,
                        AuteurId = tech2.Id,
                        Message = "Profilage des performances terminé. Les requêtes BDD sont le principal goulot. Implémenter une couche de cache Redis et optimiser les requêtes devrait résoudre le problème.",
                        DateCreation = DateTime.Now.AddHours(-24)
                    }
                };

                context.Commentaires.AddRange(comments);
                context.SaveChanges();
            }
            }
            catch (Exception ex)
            {
                // En cas d'erreur, afficher dans la console pour débogage
                Console.WriteLine($"Erreur lors du seeding : {ex.Message}");
                Console.WriteLine($"Stack trace : {ex.StackTrace}");
                // Ne pas relancer l'exception pour éviter que l'app crashe au démarrage
            }
        }
    }
}
