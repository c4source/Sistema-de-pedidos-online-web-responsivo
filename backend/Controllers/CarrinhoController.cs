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
    [Authorize(Roles = "Cliente,Colaborador")]
    public class CarrinhoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CarrinhoController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("{idCliente}")]
        public async Task<ActionResult<CarrinhoResponseDto>> GetCarrinho(int idCliente)
        {
            if (!await UsuarioPodeAcessarCliente(idCliente))
                return Forbid();

            var carrinho = await ObterOuCriarCarrinho(idCliente);
            return Ok(MontarRespostaCarrinho(carrinho));
        }

        [HttpPost("{idCliente}/itens")]
        public async Task<ActionResult<CarrinhoResponseDto>> AdicionarItem(int idCliente, AdicionarItemCarrinhoDto dto)
        {
            if (!await UsuarioPodeAcessarCliente(idCliente))
                return Forbid();

            if (dto.Quantidade <= 0)
                return BadRequest("A quantidade precisa ser maior que zero.");

            var produto = await _context.Produto.FindAsync(dto.CodProd);
            if (produto == null)
                return NotFound("Produto não encontrado.");

            if (produto.Estoque.HasValue && produto.Estoque.Value < dto.Quantidade)
                return BadRequest($"Estoque insuficiente para o produto: {produto.Nome}");

            var carrinho = await ObterOuCriarCarrinho(idCliente);
            var itemExistente = carrinho.Itens.FirstOrDefault(i => i.CodProd == dto.CodProd);

            if (itemExistente == null)
            {
                carrinho.Itens.Add(new ItemCarrinho
                {
                    CodProd = dto.CodProd,
                    Quantidade = dto.Quantidade,
                    Observacoes = dto.Observacoes
                });
            }
            else
            {
                var novaQuantidade = itemExistente.Quantidade + dto.Quantidade;
                if (produto.Estoque.HasValue && produto.Estoque.Value < novaQuantidade)
                    return BadRequest($"Estoque insuficiente para o produto: {produto.Nome}");

                itemExistente.Quantidade = novaQuantidade;
                itemExistente.Observacoes = dto.Observacoes ?? itemExistente.Observacoes;
            }

            carrinho.AtualizadoEm = DateTime.Now;
            await _context.SaveChangesAsync();

            carrinho = await ObterCarrinhoCompleto(idCliente);
            return Ok(MontarRespostaCarrinho(carrinho));
        }

        [HttpPut("{idCliente}/itens/{idItem}")]
        public async Task<ActionResult<CarrinhoResponseDto>> AtualizarItem(int idCliente, int idItem, AtualizarItemCarrinhoDto dto)
        {
            if (!await UsuarioPodeAcessarCliente(idCliente))
                return Forbid();

            if (dto.Quantidade <= 0)
                return BadRequest("A quantidade precisa ser maior que zero.");

            var carrinho = await ObterCarrinhoCompleto(idCliente);
            var item = carrinho.Itens.FirstOrDefault(i => i.Id == idItem);

            if (item == null)
                return NotFound("Item não encontrado no carrinho.");

            var produto = await _context.Produto.FindAsync(item.CodProd);
            if (produto == null)
                return NotFound("Produto não encontrado.");

            if (produto.Estoque.HasValue && produto.Estoque.Value < dto.Quantidade)
                return BadRequest($"Estoque insuficiente para o produto: {produto.Nome}");

            item.Quantidade = dto.Quantidade;
            item.Observacoes = dto.Observacoes;
            carrinho.AtualizadoEm = DateTime.Now;

            await _context.SaveChangesAsync();

            carrinho = await ObterCarrinhoCompleto(idCliente);
            return Ok(MontarRespostaCarrinho(carrinho));
        }

        [HttpDelete("{idCliente}/itens/{idItem}")]
        public async Task<IActionResult> RemoverItem(int idCliente, int idItem)
        {
            if (!await UsuarioPodeAcessarCliente(idCliente))
                return Forbid();

            var carrinho = await ObterCarrinhoCompleto(idCliente);
            var item = carrinho.Itens.FirstOrDefault(i => i.Id == idItem);

            if (item == null)
                return NotFound("Item não encontrado no carrinho.");

            _context.ItemCarrinho.Remove(item);
            carrinho.AtualizadoEm = DateTime.Now;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{idCliente}/limpar")]
        public async Task<IActionResult> LimparCarrinho(int idCliente)
        {
            if (!await UsuarioPodeAcessarCliente(idCliente))
                return Forbid();

            var carrinho = await ObterCarrinhoCompleto(idCliente);

            _context.ItemCarrinho.RemoveRange(carrinho.Itens);
            carrinho.AtualizadoEm = DateTime.Now;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private async Task<Carrinho> ObterOuCriarCarrinho(int idCliente)
        {
            var carrinho = await ObterCarrinhoCompleto(idCliente);

            if (carrinho.Id != 0)
                return carrinho;

            carrinho = new Carrinho { IdCliente = idCliente };
            _context.Carrinho.Add(carrinho);
            await _context.SaveChangesAsync();

            return await ObterCarrinhoCompleto(idCliente);
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

        private async Task<Carrinho> ObterCarrinhoCompleto(int idCliente)
        {
            return await _context.Carrinho
                       .Include(c => c.Itens)
                       .ThenInclude(i => i.Produto)
                       .FirstOrDefaultAsync(c => c.IdCliente == idCliente)
                   ?? new Carrinho { IdCliente = idCliente };
        }

        private static CarrinhoResponseDto MontarRespostaCarrinho(Carrinho carrinho)
        {
            var itens = carrinho.Itens.Select(item => new ItemCarrinhoResponseDto
            {
                Id = item.Id,
                CodProd = item.CodProd,
                Produto = item.Produto?.Nome,
                PrecoUnitario = item.Produto?.Preco ?? 0,
                Quantidade = item.Quantidade,
                Observacoes = item.Observacoes,
                Subtotal = item.Quantidade * (item.Produto?.Preco ?? 0)
            }).ToList();

            return new CarrinhoResponseDto
            {
                Id = carrinho.Id,
                IdCliente = carrinho.IdCliente,
                DataCriacao = carrinho.DataCriacao,
                AtualizadoEm = carrinho.AtualizadoEm,
                Itens = itens,
                TotalProdutos = itens.Sum(i => i.Subtotal)
            };
        }
    }
}
