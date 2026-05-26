using Pim.DTOs;
using Pim.Models;
using Pim.Repositories;

namespace Pim.Services
{
    public class PedidoService
    {
        private readonly PedidoRepository _pedidoRepository;

        public PedidoService(PedidoRepository pedidoRepository)
        {
            _pedidoRepository = pedidoRepository;
        }

        public async Task<PedidoResultadoDto> CheckoutMvpAsync(CheckoutMvpDto dto)
        {
            var tipoEntrega = NormalizarTipoEntrega(dto.TipoEntrega);

            if (string.IsNullOrWhiteSpace(dto.NomeCliente))
                return PedidoResultadoDto.Falha("Nome do cliente e obrigatorio.");

            if (string.IsNullOrWhiteSpace(dto.TelefoneCliente))
                return PedidoResultadoDto.Falha("Telefone do cliente e obrigatorio.");

            if (tipoEntrega == null)
                return PedidoResultadoDto.Falha("Tipo de entrega invalido. Use: entrega ou retirada.");

            if (tipoEntrega == TipoEntrega.Entrega &&
                (string.IsNullOrWhiteSpace(dto.Rua) ||
                 string.IsNullOrWhiteSpace(dto.Numero) ||
                 string.IsNullOrWhiteSpace(dto.Bairro)))
            {
                return PedidoResultadoDto.Falha("Rua, numero e bairro sao obrigatorios para entrega.");
            }

            if (dto.Itens == null || !dto.Itens.Any())
                return PedidoResultadoDto.Falha("O pedido precisa ter pelo menos um item.");

            if (!FormaPagamentoValida(dto.FormaPagamento))
                return PedidoResultadoDto.Falha("Forma de pagamento invalida. Use: dinheiro, cartao ou pix.");

            var pedido = new Pedido
            {
                Codigo = $"PED-{DateTime.Now:yyyyMMddHHmmssfff}",
                NomeCliente = dto.NomeCliente.Trim(),
                TelefoneCliente = dto.TelefoneCliente.Trim(),
                TipoEntrega = tipoEntrega,
                RuaEntrega = tipoEntrega == TipoEntrega.Entrega ? dto.Rua?.Trim() : null,
                NumeroEntrega = tipoEntrega == TipoEntrega.Entrega ? dto.Numero?.Trim() : null,
                BairroEntrega = tipoEntrega == TipoEntrega.Entrega ? dto.Bairro?.Trim() : null,
                ComplementoEntrega = tipoEntrega == TipoEntrega.Entrega ? dto.Complemento?.Trim() : null,
                Observacoes = dto.Observacoes,
                DataHora = DateTime.Now, 
                Status = PedidoStatus.Recebido
            };

            decimal totalProdutos = 0;

            foreach (var itemDto in dto.Itens)
            {
                if (itemDto.IdProduto <= 0)
                    return PedidoResultadoDto.Falha("Todos os itens precisam informar um produto.");

                if (itemDto.Quantidade <= 0)
                    return PedidoResultadoDto.Falha("Todos os itens precisam ter quantidade maior que zero.");

                var produto = await _pedidoRepository.BuscarProdutoPorIdAsync(itemDto.IdProduto);

                if (produto == null)
                    return PedidoResultadoDto.Falha($"Produto {itemDto.IdProduto} nao encontrado.");

                if (produto.Estoque.HasValue && produto.Estoque.Value < itemDto.Quantidade)
                    return PedidoResultadoDto.Falha($"Estoque insuficiente para o produto: {produto.Nome}");

                var itemPedido = new ItemPedido
                {
                    CodProd = itemDto.IdProduto,
                    Quantidade = itemDto.Quantidade,
                    PrecoUnitario = produto.Preco,
                    Subtotal = itemDto.Quantidade * produto.Preco
                };

                pedido.Itens.Add(itemPedido);
                totalProdutos += itemPedido.Subtotal;

                if (produto.Estoque.HasValue)
                    produto.Estoque -= itemDto.Quantidade;
            }

            pedido.ValorTotal = totalProdutos;

            pedido.Pagamento = new Pagamento
            {
                FormaPagamento = dto.FormaPagamento!.Trim().ToLower(),
                StatusPagamento = "pendente",
                ValorPago = pedido.ValorTotal,
                DataHoraPagamento = DateTime.Now
            };

            await _pedidoRepository.SalvarPedidoAsync(pedido);

            return PedidoResultadoDto.Ok(pedido);
        }

        private static string? NormalizarTipoEntrega(string? tipoEntrega)
        {
            if (string.Equals(tipoEntrega?.Trim(), TipoEntrega.Entrega, StringComparison.CurrentCultureIgnoreCase))
                return TipoEntrega.Entrega;

            if (string.Equals(tipoEntrega?.Trim(), TipoEntrega.Retirada, StringComparison.CurrentCultureIgnoreCase))
                return TipoEntrega.Retirada;

            return null;
        }

        private static bool FormaPagamentoValida(string? formaPagamento)
        {
            var forma = formaPagamento?.Trim().ToLower();
            return forma == "dinheiro" || forma == "cartao" || forma == "pix";
        }
    }
}