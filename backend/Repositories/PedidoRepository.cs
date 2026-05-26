using Microsoft.EntityFrameworkCore;
using Pim.Data;
using Pim.Models;

namespace Pim.Repositories
{
    public class PedidoRepository
    {
        private readonly AppDbContext _context;

        public PedidoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Produto?> BuscarProdutoPorIdAsync(int idProduto)
        {
            return await _context.Produto.FindAsync(idProduto);
        }

        public async Task SalvarPedidoAsync(Pedido pedido)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                _context.Pedido.Add(pedido);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}