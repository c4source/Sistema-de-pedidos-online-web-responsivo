using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pim.Data;
using Pim.DTOs;
using Pim.Models;

namespace Pim.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Colaborador")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ClientesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClienteResponseDto>>> GetClientes()
        {
            return await _context.Clientes
                .Select(cliente => new ClienteResponseDto
                {
                    Id = cliente.Id,
                    Nome = cliente.Nome,
                    Cpf = cliente.Cpf,
                    Celular = cliente.Celular,
                    Email = cliente.Email
                })
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ClienteResponseDto>> GetCliente(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);

            if (cliente == null)
                return NotFound("Cliente nao encontrado.");

            return ToResponse(cliente);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<ClienteResponseDto>> PostCliente(ClienteCreateDto dto)
        {
            var emailJaExiste = await _context.Clientes.AnyAsync(c => c.Email == dto.Email);
            if (emailJaExiste)
                return BadRequest("Ja existe um cliente cadastrado com este e-mail.");

            var cliente = new Cliente
            {
                Nome = dto.Nome,
                Cpf = dto.Cpf,
                Celular = dto.Celular,
                Email = dto.Email,
                Senha = BCrypt.Net.BCrypt.HashPassword(dto.Senha)
            };

            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCliente), new { id = cliente.Id }, ToResponse(cliente));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCliente(int id, ClienteUpdateDto dto)
        {
            var cliente = await _context.Clientes.FindAsync(id);

            if (cliente == null)
                return NotFound("Cliente nao encontrado.");

            cliente.Nome = dto.Nome;
            cliente.Cpf = dto.Cpf;
            cliente.Celular = dto.Celular;
            cliente.Email = dto.Email;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCliente(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
                return NotFound();

            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private static ClienteResponseDto ToResponse(Cliente cliente)
        {
            return new ClienteResponseDto
            {
                Id = cliente.Id,
                Nome = cliente.Nome,
                Cpf = cliente.Cpf,
                Celular = cliente.Celular,
                Email = cliente.Email
            };
        }
    }
}
