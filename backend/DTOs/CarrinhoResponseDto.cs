namespace Pim.DTOs
{
    public class CarrinhoResponseDto
    {
        public int Id { get; set; }
        public int IdCliente { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime AtualizadoEm { get; set; }
        public List<ItemCarrinhoResponseDto> Itens { get; set; } = new();
        public decimal TotalProdutos { get; set; }
    }
}
