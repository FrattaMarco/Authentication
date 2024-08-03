namespace Authentication.Application.Configurations
{
    public class TokenConfigs
    {
        public string Secret { get; set; } = null!;
        public int TokenExpirationInMinutes { get; set; }
        public string Issuer { get; set; } = null!;
    }
}
