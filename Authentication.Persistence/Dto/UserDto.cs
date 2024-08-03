using Authentication.Application.Model;

namespace Authentication.Persistence.Dto
{
    public class UserDto
    {
        public int IdUtente { get; set; }
        public string Email { get; set; } = null!;
        public byte[] Password { get; set; } = null!;
        public string Nome { get; set; } = null!;
        public string Cognome { get; set; } = null!;
        public string CodiceFiscale { get; set; } = null!;
        public string Indirizzo { get; set; } = null!;
        public DateTime DataNascita { get; set; }

        public static implicit operator UserModel(UserDto dto)
        {
            return new UserModel
            {
                IdUtente = dto.IdUtente,
                CodiceFiscale = dto.CodiceFiscale,
                Cognome = dto.Cognome,
                Email = dto.Email,
                Nome = dto.Nome,
                Password = dto.Password,
                DataNascita = dto.DataNascita,
                Indirizzo = dto.Indirizzo,
            };
        }
    }
}
