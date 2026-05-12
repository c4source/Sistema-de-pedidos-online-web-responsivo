namespace Pim.DTOs
{
    public class ResetSenhaDto
    {
        public string Email { get; set; }
        public string Cpf { get; set; } // Usaremos o CPF como prova de identidade
        public string NovaSenha { get; set; }
    }
}