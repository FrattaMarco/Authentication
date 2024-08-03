namespace Authentication.Application.CustomExceptions
{
    public class AuthenticationNotFoundException : Exception
    {
        public AuthenticationNotFoundException()
        {
        }

        public AuthenticationNotFoundException(string message)
            : base(message)
        {
        }

        public AuthenticationNotFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
