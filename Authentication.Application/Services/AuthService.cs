using Authentication.Application.Configurations;
using Authentication.Application.CustomExceptions;
using Authentication.Application.Model;
using Authentication.Application.NSwag;
using Authentication.Application.Repositories;
using ClientUsers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Authentication.Application.Services
{
    public class AuthService(IAuthenticationRepository repos, IOptionsSnapshot<TokenConfigs> tokenConfigs, ILogger<AuthService> logger, IUsersClientFactory usersClientFactory) : IAuthService
    {
        private readonly IAuthenticationRepository _repos = repos ?? throw new ArgumentNullException(nameof(repos));
        private readonly TokenConfigs _tokenConfigs = tokenConfigs.Value ?? throw new ArgumentNullException(nameof(tokenConfigs));
        private readonly ILogger<AuthService> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        private readonly IUsersClient _usersClient = usersClientFactory.Create();

        public async Task<string?> GetToken(string email, string password)
        {
            bool check = await _usersClient.CheckIfUserExistsAsync(email);
            if(!check)
            {
                throw new AuthenticationNotFoundException($"Nessun utente trovato per l'email {email}");
            }

            UserModel? user = await _repos.GetUtente(email);
            _logger.LogInformation("Recuperato utente con email {email}", email);

            //Hashing delle password: se gli hash della pswd di input e quella del db sono
            //uguali, allora procedo con la creazione del token
            string hashedPassword = HashPassword(password);
            string hexHashFromDb = BitConverter.ToString(user!.Password).Replace("-", "");
            return !CompareHash(hashedPassword, hexHashFromDb)
                ? null
                : ComposeToken(user);
        }

        public string ComposeToken(UserModel user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_tokenConfigs.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new[]
                {
                        new Claim("userId", $"{user.IdUtente}"),
                        new Claim("email", user.Email),
                        new Claim("nomeCognome", $"{user.Nome} {user.Cognome}")
                    }),
                Expires = DateTime.UtcNow.AddMinutes(_tokenConfigs.TokenExpirationInMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _tokenConfigs.Issuer
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public static string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            byte[] bytes = Encoding.Unicode.GetBytes(password);
            byte[] hashBytes = sha256.ComputeHash(bytes);
            return BitConverter.ToString(hashBytes).Replace("-", "");
        }

        public static bool CompareHash(string hashPswInput, string hashPswDb)
        {
            return hashPswInput == hashPswDb;
        }
    }
}
