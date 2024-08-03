namespace Authentication.Application.CustomExceptions
{
    public class AuthenticationUnauthorizedException : Exception
    {
        public AuthenticationUnauthorizedException()
        {
        }

        public AuthenticationUnauthorizedException(string message)
            : base(message)
        {
        }

        public AuthenticationUnauthorizedException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
