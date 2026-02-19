using GestionAnomalies.Entities;
using GestionAnomalies.Models;

namespace GestionAnomalies.Services
{
    public interface ILoginService
    {
        Task<AuthenticationResult> AuthenticateAsync(LoginViewModel model);
    }

    public class AuthenticationResult
    {
        public bool IsSuccess { get; set; }
        public User? User { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
