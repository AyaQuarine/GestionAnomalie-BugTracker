using System.IO;
using Microsoft.AspNetCore.Http;
using GestionAnomalies.Entities;
using GestionAnomalies.Repositories.Interfaces;
using GestionAnomalies.Services.Interfaces;

namespace GestionAnomalies.Services
{
    public class PieceJointeService : IPieceJointeService
    {
        private readonly IPieceJointeRepository _pieceJointeRepo;
        private static readonly string[] AllowedExtensions = { ".pdf", ".jpg", ".jpeg", ".png", ".doc", ".docx", ".txt", ".zip" };
        private const long MaxFileSize = 10 * 1024 * 1024; // 10 MB

        public PieceJointeService(IPieceJointeRepository pieceJointeRepo)
        {
            _pieceJointeRepo = pieceJointeRepo;
        }

        public async Task<IEnumerable<PieceJointe>> GetByAnomalieIdAsync(int anomalieId)
        {
            return await _pieceJointeRepo.GetByAnomalieIdAsync(anomalieId);
        }

        public async Task<bool> UploadAsync(int anomalieId, IFormFile file, string uploadsPath)
        {
            // RÈGLE MÉTIER : Validation du fichier
            if (file == null || file.Length == 0)
            {
                return false;
            }

            // Validation de la taille
            if (file.Length > MaxFileSize)
            {
                throw new InvalidOperationException($"Le fichier dépasse la taille maximale autorisée de {MaxFileSize / 1024 / 1024} MB.");
            }

            // Validation de l'extension
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!AllowedExtensions.Contains(extension))
            {
                throw new InvalidOperationException($"Type de fichier non autorisé. Extensions autorisées : {string.Join(", ", AllowedExtensions)}");
            }

            // Génération d'un nom unique pour éviter les collisions
            var uniqueFileName = $"{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(uploadsPath, uniqueFileName);

            // Création du dossier si nécessaire
            Directory.CreateDirectory(uploadsPath);

            // Sauvegarde du fichier sur disque
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Enregistrement en base de données
            var pieceJointe = new PieceJointe
            {
                AnomalieId = anomalieId,
                NomFichier = file.FileName, // Nom original
                CheminFichier = uniqueFileName, // Nom unique sur disque
                Taille = file.Length,
                DateCreation = DateTime.Now
            };

            await _pieceJointeRepo.AddAsync(pieceJointe);
            return true;
        }

        public async Task<bool> DeleteAsync(int id, string uploadsPath)
        {
            var piecesJointes = await _pieceJointeRepo.GetByAnomalieIdAsync(0);
            var pieceJointe = piecesJointes.FirstOrDefault(p => p.Id == id);

            if (pieceJointe == null)
            {
                return false;
            }

            // Suppression du fichier physique
            var filePath = Path.Combine(uploadsPath, pieceJointe.CheminFichier);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            // Suppression en base de données
            await _pieceJointeRepo.DeleteAsync(id);
            return true;
        }

        public async Task<(byte[]? fileBytes, string fileName, string contentType)> DownloadAsync(int id, string uploadsPath)
        {
            var piecesJointes = await _pieceJointeRepo.GetByAnomalieIdAsync(0);
            var pieceJointe = piecesJointes.FirstOrDefault(p => p.Id == id);

            if (pieceJointe == null)
            {
                return (null, string.Empty, string.Empty);
            }

            var filePath = Path.Combine(uploadsPath, pieceJointe.CheminFichier);
            
            if (!File.Exists(filePath))
            {
                return (null, string.Empty, string.Empty);
            }

            var fileBytes = await File.ReadAllBytesAsync(filePath);
            var contentType = GetContentType(pieceJointe.NomFichier);

            return (fileBytes, pieceJointe.NomFichier, contentType);
        }

        private string GetContentType(string fileName)
        {
            var extension = Path.GetExtension(fileName).ToLowerInvariant();
            return extension switch
            {
                ".pdf" => "application/pdf",
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".doc" => "application/msword",
                ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                ".txt" => "text/plain",
                ".zip" => "application/zip",
                _ => "application/octet-stream"
            };
        }
    }
}
