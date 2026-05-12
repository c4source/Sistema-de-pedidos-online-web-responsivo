namespace Pim.DTOs
{
    public class ProdutoUpdateDto
    {
        public string Nome { get; set; } = string.Empty;
        public decimal Preco { get; set; }
        public string? Descricao { get; set; }
        public string? Categoria { get; set; }
        public string? Status { get; set; }
        public int? Estoque { get; set; }
        public string? ImagemUrl { get; set; }
    }
}
