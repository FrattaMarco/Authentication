namespace Authentication.Application.CustomExceptions
{
    public class AuthenticationBadRequestException : Exception
    {
        public AuthenticationBadRequestException()
        {
        }

        public AuthenticationBadRequestException(string message)
            : base(message)
        {
        }

        public AuthenticationBadRequestException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
