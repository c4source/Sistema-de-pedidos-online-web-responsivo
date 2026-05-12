namespace Pim.DTOs
{
    public class PedidoResponseDto
    {
        public int Id { get; set; }
        public int IdCliente { get; set; }
        public string? Observacoes { get; set; }
        public DateTime? DataHora { get; set; }
        public string Status { get; set; } = string.Empty;
        public decimal ValorTotal { get; set; }
        public string TipoEntrega { get; set; } = string.Empty;
        public string? EnderecoEntrega { get; set; }
        public decimal TaxaEntrega { get; set; }
        public int? TempoEstimadoMinutos { get; set; }
        public DateTime? AprovadoEm { get; set; }
        public DateTime? CanceladoEm { get; set; }
        public string? CanceladoPor { get; set; }
        public string? MotivoCancelamento { get; set; }
        public List<ItemPedidoResponseDto> Itens { get; set; } = new();
    }
}
