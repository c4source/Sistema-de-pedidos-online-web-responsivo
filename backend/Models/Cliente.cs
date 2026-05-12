using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pim.Models
{
    [Table("cliente")]
    public class Cliente
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id_cliente")]
        public int Id { get; set; }

        [Column("nome_cliente")]
        public string Nome { get; set; }

        [Column("cpf_cliente")]
        public string Cpf { get; set; }

        [Column("celular_cliente")]
        public string Celular { get; set; }

        [Column("email_cliente")]
        public string Email { get; set; }

        [Column("senha_cliente")]
        public string Senha { get; set; }
    }
}