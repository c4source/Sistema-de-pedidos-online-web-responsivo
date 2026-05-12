using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pim.Models
{
    [Table("itemcarrinho")]
    public class ItemCarrinho
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id_item_carrinho")]
        public int Id { get; set; }

        [Column("id_carrinho")]
        public int IdCarrinho { get; set; }

        [Column("codprod")]
        public int CodProd { get; set; }

        [Column("quantidade")]
        public int Quantidade { get; set; }

        [Column("observacoes")]
        public string? Observacoes { get; set; }

        [ForeignKey("IdCarrinho")]
        public virtual Carrinho? Carrinho { get; set; }

        [ForeignKey("CodProd")]
        public virtual Produto? Produto { get; set; }
    }
}
