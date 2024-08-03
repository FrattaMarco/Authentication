using Authentication.Application.Commands;

namespace Authentication.Application.Mediator
{
    public interface IAuthenticationMediator
    {
        Task<string?> GetToken(CreateTokenCommand command);
    }
}
