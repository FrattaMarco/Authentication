namespace Authentication.Application.Model
{
    public class UserModel
    {
        public int IdUtente { get; set; }
        public string Email { get; set; } = null!;
        public byte[] Password { get; set; } = null!;
        public string Nome { get; set; } = null!;
        public string Cognome { get; set; } = null!;
        public string CodiceFiscale { get; set; } = null!;
        public string Indirizzo { get; set; } = null!;
        public DateTime DataNascita { get; set; }
    }
}
