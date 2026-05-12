using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pim.Models
{
    [Table("pedido")]
    public class Pedido
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id_pedido")]
        public int Id { get; set; }

        [Column("id_cliente")]
        public int IdCliente { get; set; }

        [Column("observacoes")]
        public string? Observacoes { get; set; }

        [Column("data_hora_pedido")]
        public DateTime? DataHora { get; set; }

        [Column("status_pedido")]
        public string Status { get; set; } = PedidoStatus.AguardandoAprovacao;

        [Column("valor_total")]
        public decimal ValorTotal { get; set; }

        [Column("tipo_entrega")]
        public string TipoEntrega { get; set; } = Models.TipoEntrega.Retirada;

        [Column("endereco_entrega")]
        public string? EnderecoEntrega { get; set; }

        [Column("taxa_entrega", TypeName = "decimal(10,2)")]
        public decimal TaxaEntrega { get; set; }

        [Column("tempo_estimado_minutos")]
        public int? TempoEstimadoMinutos { get; set; }

        [Column("aprovado_em")]
        public DateTime? AprovadoEm { get; set; }

        [Column("cancelado_em")]
        public DateTime? CanceladoEm { get; set; }

        [Column("cancelado_por")]
        public string? CanceladoPor { get; set; }

        [Column("motivo_cancelamento")]
        public string? MotivoCancelamento { get; set; }

        public List<ItemPedido> Itens { get; set; } = new List<ItemPedido>();
    }
}
