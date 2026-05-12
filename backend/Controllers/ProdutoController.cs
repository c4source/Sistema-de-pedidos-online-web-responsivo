using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pim.Data;
using Pim.DTOs;
using Pim.Models;

namespace Pim.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProdutoController(AppDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Cliente,Colaborador")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProdutoResponseDto>>> GetProduto(
            [FromQuery] string? categoria,
            [FromQuery] bool somenteDisponiveis = false)
        {
            var query = _context.Produto.AsQueryable();

            if (!string.IsNullOrWhiteSpace(categoria))
            {
                query = query.Where(p => p.Categoria != null && p.Categoria.ToLower() == categoria.ToLower());
            }

            if (somenteDisponiveis)
            {
                query = query.Where(p =>
                    p.Status == null ||
                    p.Status.ToLower() == "disponível" ||
                    p.Status.ToLower() == "disponivel");
            }

            return await query
                .OrderBy(p => p.Categoria)
                .ThenBy(p => p.Nome)
                .Select(produto => new ProdutoResponseDto
                {
                    Id = produto.Id,
                    Nome = produto.Nome,
                    Preco = produto.Preco,
                    Descricao = produto.Descricao,
                    Categoria = produto.Categoria,
                    Status = produto.Status,
                    Estoque = produto.Estoque,
                    ImagemUrl = produto.ImagemUrl
                })
                .ToListAsync();
        }

        [Authorize(Roles = "Cliente,Colaborador")]
        [HttpGet("categorias")]
        public async Task<ActionResult<IEnumerable<string>>> GetCategorias()
        {
            return await _context.Produto
                .Where(p => p.Categoria != null && p.Categoria != "")
                .Select(p => p.Categoria!)
                .Distinct()
                .OrderBy(categoria => categoria)
                .ToListAsync();
        }

        [Authorize(Roles = "Cliente,Colaborador")]
        [HttpGet("{id}")]
        public async Task<ActionResult<ProdutoResponseDto>> GetProduto(int id)
        {
            var produto = await _context.Produto.FindAsync(id);

            if (produto == null)
                return NotFound("Produto nao encontrado.");

            return ToResponse(produto);
        }

        [Authorize(Roles = "Colaborador")]
        [HttpPost]
        public async Task<ActionResult<ProdutoResponseDto>> PostProduto(ProdutoCreateDto dto)
        {
            var produto = new Produto
            {
                Nome = dto.Nome,
                Preco = dto.Preco,
                Descricao = dto.Descricao,
                Categoria = dto.Categoria,
                Status = dto.Status,
                Estoque = dto.Estoque,
                ImagemUrl = dto.ImagemUrl
            };

            _context.Produto.Add(produto);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProduto), new { id = produto.Id }, ToResponse(produto));
        }

        [Authorize(Roles = "Colaborador")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduto(int id, ProdutoUpdateDto dto)
        {
            var produto = await _context.Produto.FindAsync(id);

            if (produto == null)
                return NotFound("Produto nao encontrado.");

            produto.Nome = dto.Nome;
            produto.Preco = dto.Preco;
            produto.Descricao = dto.Descricao;
            produto.Categoria = dto.Categoria;
            produto.Status = dto.Status;
            produto.Estoque = dto.Estoque;
            produto.ImagemUrl = dto.ImagemUrl;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [Authorize(Roles = "Colaborador")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduto(int id)
        {
            var produto = await _context.Produto.FindAsync(id);
            if (produto == null)
                return NotFound();

            _context.Produto.Remove(produto);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private static ProdutoResponseDto ToResponse(Produto produto)
        {
            return new ProdutoResponseDto
            {
                Id = produto.Id,
                Nome = produto.Nome,
                Preco = produto.Preco,
                Descricao = produto.Descricao,
                Categoria = produto.Categoria,
                Status = produto.Status,
                Estoque = produto.Estoque,
                ImagemUrl = produto.ImagemUrl
            };
        }
    }
}
