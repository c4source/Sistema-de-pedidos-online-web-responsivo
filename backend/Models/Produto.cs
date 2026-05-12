using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pim.Models
{
    [Table("produto")]
    public class Produto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("codprod")]
        public int Id { get; set; }

        [Column("nome_produto")]
        public string Nome { get; set; }

        [Column("preco", TypeName = "decimal(10,2)")]
        public decimal Preco { get; set; }

        [Column("descricao")]
        public string? Descricao { get; set; } 

        [Column("categoria")]
        public string? Categoria { get; set; }

        [Column("status_disponibilidade")]
        public string? Status { get; set; }

        [Column("estoque")]
        public int? Estoque { get; set; }

        [Column("imagem_url")]
        public string? ImagemUrl { get; set; }
    }
}
