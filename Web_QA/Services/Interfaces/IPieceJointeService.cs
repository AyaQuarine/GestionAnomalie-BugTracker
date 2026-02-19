using GestionAnomalies.Entities;

namespace GestionAnomalies.Services.Interfaces
{
    public interface IPieceJointeService
    {
        Task<IEnumerable<PieceJointe>> GetByAnomalieIdAsync(int anomalieId);
        Task<bool> UploadAsync(int anomalieId, IFormFile file, string uploadsPath);
        Task<bool> DeleteAsync(int id, string uploadsPath);
        Task<(byte[]? fileBytes, string fileName, string contentType)> DownloadAsync(int id, string uploadsPath);
    }
}
