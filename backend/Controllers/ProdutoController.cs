using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pim.DTOs;
using Pim.Services;

namespace Pim.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutoController : ControllerBase
    {
        private readonly ProdutoService _produtoService;

        public ProdutoController(ProdutoService produtoService)
        {
            _produtoService = produtoService;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProdutoResponseDto>>> GetProduto(
            [FromQuery] string? categoria,
            [FromQuery] bool somenteDisponiveis = false)
        {
            var produtos = await _produtoService.ListarAsync(categoria, somenteDisponiveis);
            return Ok(produtos);
        }

        [AllowAnonymous]
        [HttpGet("categorias")]
        public async Task<ActionResult<IEnumerable<string>>> GetCategorias()
        {
            var categorias = await _produtoService.ListarCategoriasAsync();
            return Ok(categorias);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<ProdutoResponseDto>> GetProduto(int id)
        {
            var produto = await _produtoService.BuscarPorIdAsync(id);

            if (produto == null)
                return NotFound("Produto nao encontrado.");

            return Ok(produto);
        }

        [Authorize(Roles = "Colaborador")]
        [HttpPost]
        public async Task<ActionResult<ProdutoResponseDto>> PostProduto(ProdutoCreateDto dto)
        {
            var produtoCriado = await _produtoService.CriarAsync(dto);

            return CreatedAtAction(nameof(GetProduto), new { id = produtoCriado.Id }, produtoCriado);
        }

        [Authorize(Roles = "Colaborador")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduto(int id, ProdutoUpdateDto dto)
        {
            var atualizado = await _produtoService.AtualizarAsync(id, dto);

            if (!atualizado)
                return NotFound("Produto nao encontrado.");

            return NoContent();
        }

        [Authorize(Roles = "Colaborador")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduto(int id)
        {
            var removido = await _produtoService.RemoverAsync(id);

            if (!removido)
                return NotFound("Produto nao encontrado.");

            return NoContent();
        }
    }
}