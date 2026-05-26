using Microsoft.EntityFrameworkCore;
using Pim.Data;
using Pim.Models;

namespace Pim.Repositories
{
    public class ProdutoRepository
    {
        private readonly AppDbContext _context;

        public ProdutoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Produto>> ListarAsync(string? categoria, bool somenteDisponiveis)
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
                .ToListAsync();
        }

        public async Task<List<string>> ListarCategoriasAsync()
        {
            return await _context.Produto
                .Where(p => p.Categoria != null && p.Categoria != "")
                .Select(p => p.Categoria!)
                .Distinct()
                .OrderBy(categoria => categoria)
                .ToListAsync();
        }

        public async Task<Produto?> BuscarPorIdAsync(int id)
        {
            return await _context.Produto.FindAsync(id);
        }

        public async Task CriarAsync(Produto produto)
        {
            _context.Produto.Add(produto);
            await _context.SaveChangesAsync();
        }

        public async Task SalvarAlteracoesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task RemoverAsync(Produto produto)
        {
            _context.Produto.Remove(produto);
            await _context.SaveChangesAsync();
        }
    }
}