using Pim.DTOs;
using Pim.Models;
using Pim.Repositories;

namespace Pim.Services
{
    public class ColaboradorService
    {
        private readonly ColaboradorRepository _colaboradorRepository;

        public ColaboradorService(ColaboradorRepository colaboradorRepository)
        {
            _colaboradorRepository = colaboradorRepository;
        }

        public async Task<IEnumerable<ColaboradorResponseDto>> ListarAsync()
        {
            var colaboradores = await _colaboradorRepository.ListarAsync();

            return colaboradores.Select(ToResponse);
        }

        public async Task<ColaboradorResponseDto?> BuscarPorIdAsync(int id)
        {
            var colaborador = await _colaboradorRepository.BuscarPorIdAsync(id);

            if (colaborador == null)
                return null;

            return ToResponse(colaborador);
        }

        public async Task<(bool Sucesso, string? Mensagem, ColaboradorResponseDto? Colaborador)> CriarAsync(ColaboradorCreateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Nome))
                return (false, "Nome do colaborador e obrigatorio.", null);

            if (string.IsNullOrWhiteSpace(dto.Email))
                return (false, "E-mail do colaborador e obrigatorio.", null);

            if (string.IsNullOrWhiteSpace(dto.Senha))
                return (false, "Senha do colaborador e obrigatoria.", null);

            var emailJaExiste = await _colaboradorRepository.EmailExisteAsync(dto.Email);

            if (emailJaExiste)
                return (false, "Ja existe um colaborador cadastrado com este e-mail.", null);

            var colaborador = new Colaborador
            {
                Nome = dto.Nome.Trim(),
                Email = dto.Email.Trim(),
                Senha = BCrypt.Net.BCrypt.HashPassword(dto.Senha)
            };

            await _colaboradorRepository.CriarAsync(colaborador);

            return (true, null, ToResponse(colaborador));
        }

        public async Task<bool> AtualizarAsync(int id, ColaboradorUpdateDto dto)
        {
            var colaborador = await _colaboradorRepository.BuscarPorIdAsync(id);

            if (colaborador == null)
                return false;

            colaborador.Nome = dto.Nome.Trim();
            colaborador.Email = dto.Email.Trim();

            await _colaboradorRepository.AtualizarAsync(colaborador);

            return true;
        }

        public async Task<bool> RemoverAsync(int id)
        {
            var colaborador = await _colaboradorRepository.BuscarPorIdAsync(id);

            if (colaborador == null)
                return false;

            await _colaboradorRepository.RemoverAsync(id);

            return true;
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