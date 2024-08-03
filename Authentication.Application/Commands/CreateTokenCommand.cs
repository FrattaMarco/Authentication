using MediatR;

namespace Authentication.Application.Commands
{
    public class CreateTokenCommand : IRequest<string>
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
