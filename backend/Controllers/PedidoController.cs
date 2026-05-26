using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pim.Data;
using Pim.DTOs;
using Pim.Models;
using Pim.Services;

namespace Pim.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidoController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly PedidoService _pedidoService;

        public PedidoController(AppDbContext context, PedidoService pedidoService)
        {
            _context = context;
            _pedidoService = pedidoService;
        }

        [AllowAnonymous]
        [HttpPost("checkout-mvp")]
        public async Task<IActionResult> CheckoutMvp(CheckoutMvpDto dto)
        {
            try
            {
                var resultado = await _pedidoService.CheckoutMvpAsync(dto);

                if (!resultado.Sucesso)
                    return BadRequest(resultado.Mensagem);

                return Ok(new
                {
                    idPedido = resultado.IdPedido,
                    codigo = resultado.Codigo,
                    status = resultado.Status,
                    valorTotal = resultado.ValorTotal,
                    mensagem = resultado.Mensagem
                });
            }
            catch (Exception ex)
            {
                var mensagemErro = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return StatusCode(500, $"Erro Detalhado: {mensagemErro}");
            }
        }

        [Authorize(Roles = "Colaborador")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PedidoResponseDto>>> GetPedidos()
        {
            var pedidos = await _context.Pedido
                .Include(p => p.Itens)
                .ThenInclude(i => i.Produto)
                .Include(p => p.Pagamento)
                .OrderByDescending(p => p.DataHora)
                .ToListAsync();

            return pedidos.Select(ToResponse).ToList();
        }

        [Authorize(Roles = "Colaborador")]
        [HttpGet("{id}")]
        public async Task<ActionResult<PedidoResponseDto>> GetPedido(int id)
        {
            var pedido = await BuscarPedidoCompleto(id);

            if (pedido == null)
                return NotFound("Pedido nao encontrado.");

            return ToResponse(pedido);
        }

        [Authorize(Roles = "Colaborador")]
        [HttpPatch("{id}/aprovar")]
        public async Task<IActionResult> AprovarPedido(int id, AprovarPedidoDto dto)
        {
            var pedido = await _context.Pedido.FindAsync(id);

            if (pedido == null)
                return NotFound("Pedido nao encontrado.");

            if (pedido.Status == PedidoStatus.Cancelado || pedido.Status == PedidoStatus.Finalizado)
                return BadRequest("Pedido finalizado ou cancelado nao pode mudar de status.");

            pedido.Status = PedidoStatus.EmPreparo;
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = $"Pedido {id} enviado para preparo.",
                pedido.Status
            });
        }

        [Authorize(Roles = "Colaborador")]
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> AtualizarStatus(int id, AtualizarStatusPedidoDto dto)
        {
            var pedido = await _context.Pedido.FindAsync(id);

            if (pedido == null)
                return NotFound("Pedido nao encontrado.");

            var novoStatus = NormalizarStatus(dto.Status, PedidoStatus.StatusOperacionais);

            if (novoStatus == null)
                return BadRequest("Status invalido. Use: recebido, em_preparo, pronto, finalizado ou cancelado.");

            pedido.Status = novoStatus;
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = $"Status do pedido {id} atualizado para {novoStatus}."
            });
        }

        [Authorize(Roles = "Colaborador")]
        [HttpPatch("{id}/cancelar-loja")]
        public async Task<IActionResult> CancelarPelaLoja(int id, CancelarPedidoDto dto)
        {
            var pedido = await BuscarPedidoCompleto(id);

            if (pedido == null)
                return NotFound("Pedido nao encontrado.");

            if (pedido.Status == PedidoStatus.Finalizado)
                return BadRequest("Pedido finalizado nao pode ser cancelado.");

            return await CancelarPedido(pedido);
        }

        private async Task<IActionResult> CancelarPedido(Pedido pedido)
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

            if (pedido.Pagamento != null)
                pedido.Pagamento.StatusPagamento = "cancelado";

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = $"Pedido {pedido.Id} cancelado."
            });
        }

        private async Task<Pedido?> BuscarPedidoCompleto(int id)
        {
            return await _context.Pedido
                .Include(p => p.Itens)
                .ThenInclude(i => i.Produto)
                .Include(p => p.Pagamento)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        private static PedidoResponseDto ToResponse(Pedido pedido)
        {
            return new PedidoResponseDto
            {
                Id = pedido.Id,
                Codigo = pedido.Codigo,
                NomeCliente = pedido.NomeCliente,
                TelefoneCliente = pedido.TelefoneCliente,
                Observacoes = pedido.Observacoes,
                DataHora = pedido.DataHora,
                Status = pedido.Status,
                ValorTotal = pedido.ValorTotal,
                TipoEntrega = pedido.TipoEntrega,
                RuaEntrega = pedido.RuaEntrega,
                NumeroEntrega = pedido.NumeroEntrega,
                BairroEntrega = pedido.BairroEntrega,
                ComplementoEntrega = pedido.ComplementoEntrega,
                FormaPagamento = pedido.Pagamento?.FormaPagamento,
                StatusPagamento = pedido.Pagamento?.StatusPagamento,
                Itens = pedido.Itens.Select(item => new ItemPedidoResponseDto
                {
                    Id = item.Id,
                    CodProd = item.CodProd,
                    Produto = item.Produto?.Nome,
                    Quantidade = item.Quantidade,
                    PrecoUnitario = item.PrecoUnitario,
                    Subtotal = item.Subtotal
                }).ToList()
            };
        }

        private static string? NormalizarStatus(string status, IEnumerable<string> statusValidos)
        {
            return statusValidos.FirstOrDefault(s =>
                string.Equals(s, status?.Trim(), StringComparison.CurrentCultureIgnoreCase));
        }
    }
}