namespace Pim.DTOs
{
    public class CheckoutCarrinhoDto
    {
        public int IdCliente { get; set; }
        public string TipoEntrega { get; set; } = "Retirada";
        public string? EnderecoEntrega { get; set; }
        public string? Observacoes { get; set; }
    }
}
