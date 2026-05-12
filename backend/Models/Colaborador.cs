using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pim.Models
{
    [Table("colaborador")]
    public class Colaborador
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id_usuario")]
        public int Id { get; set; }

        [Column("nome_usuario")]
        public string Nome { get; set; }

        [Column("email_usuario")]
        public string Email { get; set; }

        [Column("senha_usuario")]
        public string Senha { get; set; }
    }
}