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
    [Authorize(Roles = "Colaborador")]
    public class ColaboradoresController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ColaboradoresController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ColaboradorResponseDto>>> GetColaboradores()
        {
            return await _context.Colaboradores
                .Select(colaborador => new ColaboradorResponseDto
                {
                    Id = colaborador.Id,
                    Nome = colaborador.Nome,
                    Email = colaborador.Email
                })
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ColaboradorResponseDto>> GetColaborador(int id)
        {
            var colaborador = await _context.Colaboradores.FindAsync(id);

            if (colaborador == null)
                return NotFound("Colaborador nao encontrado.");

            return ToResponse(colaborador);
        }

        [HttpPost]
        public async Task<ActionResult<ColaboradorResponseDto>> PostColaborador(ColaboradorCreateDto dto)
        {
            var emailJaExiste = await _context.Colaboradores.AnyAsync(c => c.Email == dto.Email);
            if (emailJaExiste)
                return BadRequest("Ja existe um colaborador cadastrado com este e-mail.");

            var colaborador = new Colaborador
            {
                Nome = dto.Nome,
                Email = dto.Email,
                Senha = BCrypt.Net.BCrypt.HashPassword(dto.Senha)
            };

            _context.Colaboradores.Add(colaborador);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetColaborador), new { id = colaborador.Id }, ToResponse(colaborador));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutColaborador(int id, ColaboradorUpdateDto dto)
        {
            var colaborador = await _context.Colaboradores.FindAsync(id);

            if (colaborador == null)
                return NotFound("Colaborador nao encontrado.");

            colaborador.Nome = dto.Nome;
            colaborador.Email = dto.Email;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteColaborador(int id)
        {
            var colaborador = await _context.Colaboradores.FindAsync(id);

            if (colaborador == null)
                return NotFound();

            _context.Colaboradores.Remove(colaborador);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private static ColaboradorResponseDto ToResponse(Colaborador colaborador)
        {
            return new ColaboradorResponseDto
            {
                Id = colaborador.Id,
                Nome = colaborador.Nome,
                Email = colaborador.Email
            };
        }
    }
}
