using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Pim.Data;
using Pim.DTOs;
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
        /// Login para funcionários e administradores.
        /// </summary>
        [HttpPost("login-colaborador")]
        public async Task<ActionResult> LoginColaborador([FromBody] LoginDto login)
        {
            var colaborador = await _context.Colaboradores
                .FirstOrDefaultAsync(c => c.Email == login.Email);

            if (colaborador == null || !BCrypt.Net.BCrypt.Verify(login.Senha, colaborador.Senha))
            {
                return Unauthorized("E-mail ou senha de colaborador incorretos.");
            }

            var token = GerarToken(colaborador.Nome, colaborador.Email, "Colaborador");

            return Ok(new
            {
                token,
                usuario = colaborador.Nome,
                perfil = "Colaborador"
            });
        }

        // Método privado para gerar token do colaborador.
        private string GerarToken(string nome, string email, string papel)
        {
            var jwtKey = _config["Jwt:Key"];

            if (string.IsNullOrWhiteSpace(jwtKey))
            {
                throw new InvalidOperationException("A chave JWT não foi configurada em appsettings.json.");
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, nome),
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, papel),
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