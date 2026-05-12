using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Pim.Data;
using Pim.DTOs;
using Pim.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Pim.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public AuthController(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        /// <summary>
        /// Login para funcionários e administradores
        /// </summary>
        [HttpPost("login-colaborador")]
        public async Task<ActionResult> LoginColaborador([FromBody] LoginDto login)
        {
            var colaborador = await _context.Colaboradores.FirstOrDefaultAsync(c => c.Email == login.Email);

            if (colaborador == null || !BCrypt.Net.BCrypt.Verify(login.Senha, colaborador.Senha))
            {
                return Unauthorized("E-mail ou senha de colaborador incorretos.");
            }

            var token = GerarToken(colaborador.Nome, colaborador.Email, "Colaborador");

            return Ok(new { token, usuario = colaborador.Nome, perfil = "Colaborador" });
        }

        /// <summary>
        /// Login para clientes do e-commerce
        /// </summary>
        [HttpPost("login-cliente")]
        public async Task<ActionResult> LoginCliente([FromBody] LoginDto login)
        {
            var cliente = await _context.Clientes.FirstOrDefaultAsync(c => c.Email == login.Email);

            if (cliente == null || !BCrypt.Net.BCrypt.Verify(login.Senha, cliente.Senha))
            {
                return Unauthorized("E-mail ou senha de cliente incorretos.");
            }

            var token = GerarToken(cliente.Nome, cliente.Email, "Cliente");

            return Ok(new { token, usuario = cliente.Nome, perfil = "Cliente" });
        }

        /// <summary>
        /// Redefinição de senha (Esqueci minha senha)
        /// </summary>
        [HttpPost("reset-senha-cliente")]
        public async Task<IActionResult> ResetSenhaCliente([FromBody] ResetSenhaDto dto)
        {
            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(c => c.Email == dto.Email && c.Cpf == dto.Cpf);

            if (cliente == null) return BadRequest("Dados de validação incorretos.");

            cliente.Senha = BCrypt.Net.BCrypt.HashPassword(dto.NovaSenha);
            await _context.SaveChangesAsync();

            return Ok("Senha redefinida com sucesso.");
        }

        // Método privado genérico para gerar tokens para ambos os perfis
        private string GerarToken(string nome, string email, string papel)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, nome),
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, papel), // Define se é Cliente ou Colaborador no Token
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(8),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}