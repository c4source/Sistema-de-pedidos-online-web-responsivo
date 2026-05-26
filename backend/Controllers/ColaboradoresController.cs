using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pim.DTOs;
using Pim.Services;

namespace Pim.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Colaborador")]
    public class ColaboradoresController : ControllerBase
    {
        private readonly ColaboradorService _colaboradorService;

        public ColaboradoresController(ColaboradorService colaboradorService)
        {
            _colaboradorService = colaboradorService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ColaboradorResponseDto>>> GetColaboradores()
        {
            var colaboradores = await _colaboradorService.ListarAsync();
            return Ok(colaboradores);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ColaboradorResponseDto>> GetColaborador(int id)
        {
            var colaborador = await _colaboradorService.BuscarPorIdAsync(id);

            if (colaborador == null)
                return NotFound("Colaborador nao encontrado.");

            return Ok(colaborador);
        }

        [HttpPost]
        public async Task<ActionResult<ColaboradorResponseDto>> PostColaborador(ColaboradorCreateDto dto)
        {
            var resultado = await _colaboradorService.CriarAsync(dto);

            if (!resultado.Sucesso)
                return BadRequest(resultado.Mensagem);

            return CreatedAtAction(
                nameof(GetColaborador),
                new { id = resultado.Colaborador!.Id },
                resultado.Colaborador
            );
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutColaborador(int id, ColaboradorUpdateDto dto)
        {
            var atualizado = await _colaboradorService.AtualizarAsync(id, dto);

            if (!atualizado)
                return NotFound("Colaborador nao encontrado.");

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteColaborador(int id)
        {
            var removido = await _colaboradorService.RemoverAsync(id);

            if (!removido)
                return NotFound("Colaborador nao encontrado.");

            return NoContent();
        }
    }
}