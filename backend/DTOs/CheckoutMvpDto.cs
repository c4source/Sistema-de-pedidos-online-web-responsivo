namespace Pim.DTOs
{
    public class CheckoutMvpDto
    {
        public string NomeCliente { get; set; } = string.Empty;
        public string TelefoneCliente { get; set; } = string.Empty;
        public string TipoEntrega { get; set; } = string.Empty;
        public string? Rua { get; set; }
        public string? Numero { get; set; }
        public string? Bairro { get; set; }
        public string? Complemento { get; set; }
        public string? Observacoes { get; set; }
        public string? FormaPagamento { get; set; }
        public List<ItemCheckoutMvpDto> Itens { get; set; } = new();
    }
}
