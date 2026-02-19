using GestionAnomalies.Repositories.Interfaces;
using GestionAnomalies.Models;

namespace GestionAnomalies.Services
{
    public class LoginService : ILoginService
    {
        private readonly IUserRepository _userRepository;

        public LoginService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<AuthenticationResult> AuthenticateAsync(LoginViewModel model)
        {
            // Validation de base
            if (string.IsNullOrWhiteSpace(model.Email) || string.IsNullOrWhiteSpace(model.MotDePasse))
            {
                return new AuthenticationResult
                {
                    IsSuccess = false,
                    ErrorMessage = "Email et mot de passe sont obligatoires."
                };
            }

            // Récupération du user par email
            var user = await _userRepository.GetByEmailAsync(model.Email);

            // Vérification du mot de passe (à remplacer par un Hash en production)
            if (user == null || user.MotDePasse != model.MotDePasse)
            {
                return new AuthenticationResult
                {
                    IsSuccess = false,
                    ErrorMessage = "Email ou mot de passe incorrect."
                };
            }

            // Authentification réussie
            return new AuthenticationResult
            {
                IsSuccess = true,
                User = user
            };
        }
    }
}
