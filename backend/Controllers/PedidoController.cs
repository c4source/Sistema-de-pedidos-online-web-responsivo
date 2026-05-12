using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pim.Data;
using Pim.DTOs;
using Pim.Models;
using System.Security.Claims;

namespace Pim.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidoController : ControllerBase
    {
        private const decimal TaxaEntregaFixa = 10m;
        private readonly AppDbContext _context;

        public PedidoController(AppDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Colaborador")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PedidoResponseDto>>> GetPedidos()
        {
            var pedidos = await _context.Pedido
                                 .Include(p => p.Itens)
                                 .ThenInclude(i => i.Produto)
                                 .OrderByDescending(p => p.DataHora)
                                 .ToListAsync();

            return pedidos.Select(ToResponse).ToList();
        }

        [Authorize(Roles = "Cliente,Colaborador")]
        [HttpGet("{id}")]
        public async Task<ActionResult<PedidoResponseDto>> GetPedido(int id)
        {
            var pedido = await BuscarPedidoCompleto(id);

            if (pedido == null)
                return NotFound("Pedido nao encontrado.");

            if (!await UsuarioPodeAcessarCliente(pedido.IdCliente))
                return Forbid();

            return ToResponse(pedido);
        }

        [Authorize(Roles = "Cliente,Colaborador")]
        [HttpGet("cliente/{idCliente}")]
        public async Task<ActionResult<IEnumerable<PedidoResponseDto>>> GetPedidosDoCliente(int idCliente)
        {
            if (!await UsuarioPodeAcessarCliente(idCliente))
                return Forbid();

            var pedidos = await _context.Pedido
                                 .Include(p => p.Itens)
                                 .ThenInclude(i => i.Produto)
                                 .Where(p => p.IdCliente == idCliente)
                                 .OrderByDescending(p => p.DataHora)
                                 .ToListAsync();

            return pedidos.Select(ToResponse).ToList();
        }

        [Authorize(Roles = "Cliente,Colaborador")]
        [HttpPost("checkout-carrinho")]
        public async Task<IActionResult> CheckoutCarrinho(CheckoutCarrinhoDto dto)
        {
            if (!await UsuarioPodeAcessarCliente(dto.IdCliente))
                return Forbid();

            var carrinho = await _context.Carrinho
                                         .Include(c => c.Itens)
                                         .FirstOrDefaultAsync(c => c.IdCliente == dto.IdCliente);

            if (carrinho == null || !carrinho.Itens.Any())
                return BadRequest("O carrinho esta vazio.");

            var pedido = new Pedido
            {
                IdCliente = dto.IdCliente,
                TipoEntrega = dto.TipoEntrega,
                EnderecoEntrega = dto.EnderecoEntrega,
                Observacoes = dto.Observacoes,
                Itens = carrinho.Itens.Select(item => new ItemPedido
                {
                    CodProd = item.CodProd,
                    Quantidade = item.Quantidade,
                    Observacoes = item.Observacoes
                }).ToList()
            };

            var resultado = await CriarPedido(pedido);

            if (resultado is ObjectResult objectResult && objectResult.StatusCode >= 400)
                return resultado;

            _context.ItemCarrinho.RemoveRange(carrinho.Itens);
            _context.Carrinho.Remove(carrinho);
            await _context.SaveChangesAsync();

            return resultado;
        }

        [Authorize(Roles = "Colaborador")]
        [HttpPost("checkout")]
        public async Task<IActionResult> Checkout(Pedido pedido)
        {
            return await CriarPedido(pedido);
        }

        [Authorize(Roles = "Colaborador")]
        [HttpPatch("{id}/aprovar")]
        public async Task<IActionResult> AprovarPedido(int id, AprovarPedidoDto dto)
        {
            var pedido = await _context.Pedido.FindAsync(id);

            if (pedido == null)
                return NotFound("Pedido nao encontrado.");

            if (pedido.Status == PedidoStatus.Cancelado)
                return BadRequest("Pedido cancelado nao pode ser aprovado.");

            if (!EstaAguardandoAprovacao(pedido.Status))
                return BadRequest("Apenas pedidos aguardando aprovacao podem ser aprovados.");

            pedido.Status = PedidoStatus.Aprovado;
            pedido.AprovadoEm = DateTime.Now;
            pedido.TempoEstimadoMinutos = dto.TempoEstimadoMinutos;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = $"Pedido {id} aprovado.",
                pedido.Status,
                pedido.TempoEstimadoMinutos
            });
        }

        [Authorize(Roles = "Colaborador")]
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> AtualizarStatus(int id, AtualizarStatusPedidoDto dto)
        {
            var pedido = await _context.Pedido.FindAsync(id);

            if (pedido == null)
                return NotFound("Pedido nao encontrado.");

            if (pedido.Status == PedidoStatus.Cancelado)
                return BadRequest("Pedido cancelado nao pode mudar de status.");

            var novoStatus = NormalizarStatus(dto.Status, PedidoStatus.StatusOperacionais);
            if (novoStatus == null)
                return BadRequest("Status invalido. Use: Aprovado, Em Preparacao, Pronto para Retirada, Saiu para Entrega, Entregue ou Retirado.");

            if (EstaAguardandoAprovacao(pedido.Status) && novoStatus != PedidoStatus.Aprovado)
                return BadRequest("A loja precisa aprovar o pedido antes de avancar o status.");

            if (novoStatus == PedidoStatus.SaiuParaEntrega && pedido.TipoEntrega != TipoEntrega.Entrega)
                return BadRequest("Somente pedidos de entrega podem usar o status 'Saiu para Entrega'.");

            if (novoStatus == PedidoStatus.Entregue && pedido.TipoEntrega != TipoEntrega.Entrega)
                return BadRequest("Somente pedidos de entrega podem usar o status 'Entregue'.");

            pedido.Status = novoStatus;

            if (novoStatus == PedidoStatus.Aprovado && pedido.AprovadoEm == null)
                pedido.AprovadoEm = DateTime.Now;

            await _context.SaveChangesAsync();

            return Ok(new { message = $"Status do pedido {id} atualizado para {novoStatus}." });
        }

        [Authorize(Roles = "Cliente")]
        [HttpPatch("{id}/cancelar-cliente")]
        public async Task<IActionResult> CancelarPeloCliente(int id, CancelarPedidoDto dto)
        {
            var pedido = await BuscarPedidoCompleto(id);

            if (pedido == null)
                return NotFound("Pedido nao encontrado.");

            if (!await UsuarioPodeAcessarCliente(pedido.IdCliente))
                return Forbid();

            if (pedido.Status != PedidoStatus.AguardandoAprovacao && pedido.Status != PedidoStatus.Aprovado)
                return BadRequest("O cliente so pode cancelar pedidos antes da preparacao comecar.");

            return await CancelarPedido(pedido, "Cliente", dto.Motivo);
        }

        [Authorize(Roles = "Colaborador")]
        [HttpPatch("{id}/cancelar-loja")]
        public async Task<IActionResult> CancelarPelaLoja(int id, CancelarPedidoDto dto)
        {
            var pedido = await BuscarPedidoCompleto(id);

            if (pedido == null)
                return NotFound("Pedido nao encontrado.");

            if (pedido.Status == PedidoStatus.Entregue || pedido.Status == PedidoStatus.Retirado)
                return BadRequest("Pedido finalizado nao pode ser cancelado.");

            return await CancelarPedido(pedido, "Loja", dto.Motivo);
        }

        private async Task<IActionResult> CriarPedido(Pedido pedido)
        {
            if (!TipoEntrega.Todos.Contains(pedido.TipoEntrega))
                return BadRequest("Tipo de entrega invalido. Use: Retirada ou Entrega.");

            if (pedido.TipoEntrega == TipoEntrega.Entrega && string.IsNullOrWhiteSpace(pedido.EnderecoEntrega))
                return BadRequest("Informe o endereco para pedidos com entrega.");

            if (pedido.Itens == null || !pedido.Itens.Any())
                return BadRequest("O pedido precisa ter pelo menos um item.");

            pedido.TaxaEntrega = pedido.TipoEntrega == TipoEntrega.Entrega ? TaxaEntregaFixa : 0;

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                decimal totalProdutos = 0;

                foreach (var item in pedido.Itens)
                {
                    if (item.Quantidade <= 0)
                        return BadRequest("Todos os itens precisam ter quantidade maior que zero.");

                    var produto = await _context.Produto.FindAsync(item.CodProd);
                    if (produto == null)
                        return BadRequest($"Produto {item.CodProd} nao encontrado.");

                    if (produto.Estoque.HasValue && produto.Estoque.Value < item.Quantidade)
                        return BadRequest($"Estoque insuficiente para o produto: {produto.Nome}");

                    item.PrecoUnitario = produto.Preco;
                    item.Subtotal = item.Quantidade * item.PrecoUnitario;
                    totalProdutos += item.Subtotal;

                    if (produto.Estoque.HasValue)
                        produto.Estoque -= item.Quantidade;
                }

                pedido.ValorTotal = totalProdutos + pedido.TaxaEntrega;
                pedido.DataHora = DateTime.Now;
                pedido.Status = PedidoStatus.AguardandoAprovacao;

                _context.Pedido.Add(pedido);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return Ok(new
                {
                    message = "Pedido recebido e aguardando aprovacao da loja.",
                    id_pedido = pedido.Id,
                    pedido.Status,
                    pedido.ValorTotal
                });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                var mensagemErro = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return StatusCode(500, $"Erro Detalhado: {mensagemErro}");
            }
        }

        private async Task<IActionResult> CancelarPedido(Pedido pedido, string canceladoPor, string? motivo)
        {
            if (pedido.Status == PedidoStatus.Cancelado)
                return BadRequest("Pedido ja esta cancelado.");

            foreach (var item in pedido.Itens)
            {
                var produto = item.Produto ?? await _context.Produto.FindAsync(item.CodProd);

                if (produto?.Estoque != null)
                    produto.Estoque += item.Quantidade;
            }

            pedido.Status = PedidoStatus.Cancelado;
            pedido.CanceladoEm = DateTime.Now;
            pedido.CanceladoPor = canceladoPor;
            pedido.MotivoCancelamento = motivo;

            await _context.SaveChangesAsync();

            return Ok(new { message = $"Pedido {pedido.Id} cancelado por {canceladoPor}." });
        }

        private async Task<Pedido?> BuscarPedidoCompleto(int id)
        {
            return await _context.Pedido
                                 .Include(p => p.Itens)
                                 .ThenInclude(i => i.Produto)
                                 .FirstOrDefaultAsync(p => p.Id == id);
        }

        private static PedidoResponseDto ToResponse(Pedido pedido)
        {
            return new PedidoResponseDto
            {
                Id = pedido.Id,
                IdCliente = pedido.IdCliente,
                Observacoes = pedido.Observacoes,
                DataHora = pedido.DataHora,
                Status = pedido.Status,
                ValorTotal = pedido.ValorTotal,
                TipoEntrega = pedido.TipoEntrega,
                EnderecoEntrega = pedido.EnderecoEntrega,
                TaxaEntrega = pedido.TaxaEntrega,
                TempoEstimadoMinutos = pedido.TempoEstimadoMinutos,
                AprovadoEm = pedido.AprovadoEm,
                CanceladoEm = pedido.CanceladoEm,
                CanceladoPor = pedido.CanceladoPor,
                MotivoCancelamento = pedido.MotivoCancelamento,
                Itens = pedido.Itens.Select(item => new ItemPedidoResponseDto
                {
                    Id = item.Id,
                    CodProd = item.CodProd,
                    Produto = item.Produto?.Nome,
                    Quantidade = item.Quantidade,
                    PrecoUnitario = item.PrecoUnitario,
                    Subtotal = item.Subtotal,
                    Observacoes = item.Observacoes
                }).ToList()
            };
        }

        private async Task<bool> UsuarioPodeAcessarCliente(int idCliente)
        {
            if (User.IsInRole("Colaborador"))
                return true;

            var email = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrWhiteSpace(email))
                return false;

            return await _context.Clientes.AnyAsync(c => c.Id == idCliente && c.Email == email);
        }

        private static string? NormalizarStatus(string status, IEnumerable<string> statusValidos)
        {
            return statusValidos.FirstOrDefault(s =>
                string.Equals(s, status?.Trim(), StringComparison.CurrentCultureIgnoreCase));
        }

        private static bool EstaAguardandoAprovacao(string status)
        {
            var statusNormalizado = status?.Trim();

            return string.Equals(statusNormalizado, PedidoStatus.AguardandoAprovacao, StringComparison.CurrentCultureIgnoreCase)
                || string.Equals(statusNormalizado, "Aguardando Aprovacao", StringComparison.CurrentCultureIgnoreCase)
                || string.Equals(statusNormalizado, "Aguardando", StringComparison.CurrentCultureIgnoreCase)
                || string.Equals(statusNormalizado, "Pendente", StringComparison.CurrentCultureIgnoreCase);
        }
    }
}
