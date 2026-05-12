using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pim.Models
{
    [Table("carrinho")]
    public class Carrinho
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id_carrinho")]
        public int Id { get; set; }

        [Column("id_cliente")]
        public int IdCliente { get; set; }

        [Column("data_criacao")]
        public DateTime DataCriacao { get; set; } = DateTime.Now;

        [Column("atualizado_em")]
        public DateTime AtualizadoEm { get; set; } = DateTime.Now;

        public List<ItemCarrinho> Itens { get; set; } = new List<ItemCarrinho>();
    }
}
