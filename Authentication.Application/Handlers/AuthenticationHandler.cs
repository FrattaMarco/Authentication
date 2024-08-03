using Authentication.Application.Commands;
using Authentication.Application.CustomExceptions;
using Authentication.Application.NSwag;
using Authentication.Application.Services;
using ClientUsers;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Authentication.Application.Handlers
{
    public class AuthenticationHandler(IAuthService authService, ILogger<AuthenticationHandler> logger, IUsersClientFactory usersClientFactory) : IRequestHandler<CreateTokenCommand, string>
    {
        private readonly IAuthService _authService = authService ?? throw new ArgumentNullException(nameof(authService));
        private readonly ILogger<AuthenticationHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        private readonly IUsersClient _usersClient = usersClientFactory.Create();
        public async Task<string> Handle(CreateTokenCommand command, CancellationToken cancellationToken)
        {
                _logger.LogInformation("Generazione token jwt per l'utente con Email {Email}", command.Email);
                string? token = await _authService.GetToken(command.Email, command.Password);

                return !string.IsNullOrEmpty(token) ? token : throw new AuthenticationBadRequestException("Password errata");
        }
    }
}