using Microsoft.EntityFrameworkCore;
using GestionAnomalies.Entities;

namespace GestionAnomalies.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // --- 1. TABLES MÉTIERS ---
        public DbSet<User> Users { get; set; }
        public DbSet<Anomalie> Anomalies { get; set; }
        public DbSet<Projet> Projets { get; set; }
        public DbSet<Commentaire> Commentaires { get; set; }
        public DbSet<PieceJointe> PiecesJointes { get; set; }
        
        // --- 2. TABLES DE RÉFÉRENCE (ADMIN) ---
        public DbSet<TypeAnomalie> TypesAnomalies { get; set; }
        public DbSet<Priorite> Priorites { get; set; }
        public DbSet<Statut> Statuts { get; set; }

        // --- 3. CONFIGURATION DES RELATIONS  (Fluent API) ---
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Une anomalie a 2 liens vers User. On désactive la suppression en cascade.

            // Relation 1 : Créateur de l'anomalie
            modelBuilder.Entity<Anomalie>()
                .HasOne(a => a.Createur)
                .WithMany(u => u.AnomaliesDeclarees)
                .HasForeignKey(a => a.CreateurId)
                .OnDelete(DeleteBehavior.Restrict); // Bloque la suppression du user s'il a créé des anomalies

            // Relation 2 : Technicien assigné
            modelBuilder.Entity<Anomalie>()
                .HasOne(a => a.Assignee)
                .WithMany(u => u.AnomaliesAssignees)
                .HasForeignKey(a => a.AssigneeId)
                .OnDelete(DeleteBehavior.Restrict); // Bloque la suppression du user s'il est assigné à des anomalies

            // Relation Auteur du commentaire
            modelBuilder.Entity<Commentaire>()
                .HasOne(c => c.Auteur)
                .WithMany(u => u.Commentaires)
                .HasForeignKey(c => c.AuteurId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}