using Npgsql;
using NpgsqlTypes;
using Pim.Models;

namespace Pim.Repositories
{
    public class PedidoRepository
    {
        private readonly string _connectionString;

        public PedidoRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' nao encontrada.");
        }

        public async Task<Produto?> BuscarProdutoPorIdAsync(int idProduto)
        {
            var sql = @"
                SELECT codprod, nome_produto, preco, descricao, categoria,
                       status_disponibilidade, estoque, imagem_url
                FROM produto
                WHERE codprod = @idProduto;
            ";

            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            await using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.Add("@idProduto", NpgsqlDbType.Integer).Value = idProduto;

            await using var reader = await command.ExecuteReaderAsync();

            if (!await reader.ReadAsync())
                return null;

            return MapearProduto(reader);
        }

        public async Task SalvarPedidoAsync(Pedido pedido)
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            await using var transaction = await connection.BeginTransactionAsync();

            try
            {
                var idPedido = await InserirPedidoAsync(connection, transaction, pedido);
                pedido.Id = idPedido;

                foreach (var item in pedido.Itens)
                {
                    item.IdPedido = idPedido;

                    await InserirItemPedidoAsync(connection, transaction, item);
                    await AtualizarEstoqueAsync(connection, transaction, item.CodProd, item.Quantidade);
                }

                if (pedido.Pagamento != null)
                {
                    pedido.Pagamento.IdPedido = idPedido;
                    await InserirPagamentoAsync(connection, transaction, pedido.Pagamento);
                }

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        private static async Task<int> InserirPedidoAsync(
            NpgsqlConnection connection,
            NpgsqlTransaction transaction,
            Pedido pedido)
        {
            var sql = @"
                INSERT INTO pedido (
                    codigo,
                    nome_cliente,
                    telefone_cliente,
                    tipo_entrega,
                    rua_entrega,
                    numero_entrega,
                    bairro_entrega,
                    complemento_entrega,
                    observacoes,
                    data_hora_pedido,
                    status_pedido,
                    valor_total
                )
                VALUES (
                    @codigo,
                    @nomeCliente,
                    @telefoneCliente,
                    @tipoEntrega,
                    @ruaEntrega,
                    @numeroEntrega,
                    @bairroEntrega,
                    @complementoEntrega,
                    @observacoes,
                    @dataHoraPedido,
                    @statusPedido,
                    @valorTotal
                )
                RETURNING id_pedido;
            ";

            await using var command = new NpgsqlCommand(sql, connection, transaction);

            command.Parameters.Add("@codigo", NpgsqlDbType.Varchar).Value =
                pedido.Codigo ?? (object)DBNull.Value;

            command.Parameters.Add("@nomeCliente", NpgsqlDbType.Varchar).Value =
                pedido.NomeCliente;

            command.Parameters.Add("@telefoneCliente", NpgsqlDbType.Varchar).Value =
                pedido.TelefoneCliente;

            command.Parameters.Add("@tipoEntrega", NpgsqlDbType.Varchar).Value =
                pedido.TipoEntrega;

            command.Parameters.Add("@ruaEntrega", NpgsqlDbType.Varchar).Value =
                pedido.RuaEntrega ?? (object)DBNull.Value;

            command.Parameters.Add("@numeroEntrega", NpgsqlDbType.Varchar).Value =
                pedido.NumeroEntrega ?? (object)DBNull.Value;

            command.Parameters.Add("@bairroEntrega", NpgsqlDbType.Varchar).Value =
                pedido.BairroEntrega ?? (object)DBNull.Value;

            command.Parameters.Add("@complementoEntrega", NpgsqlDbType.Varchar).Value =
                pedido.ComplementoEntrega ?? (object)DBNull.Value;

            command.Parameters.Add("@observacoes", NpgsqlDbType.Text).Value =
                pedido.Observacoes ?? (object)DBNull.Value;

            command.Parameters.Add("@dataHoraPedido", NpgsqlDbType.Timestamp).Value =
                pedido.DataHora;

            command.Parameters.Add("@statusPedido", NpgsqlDbType.Varchar).Value =
                pedido.Status;

            command.Parameters.Add("@valorTotal", NpgsqlDbType.Numeric).Value =
                pedido.ValorTotal;

            var result = await command.ExecuteScalarAsync();

            return Convert.ToInt32(result);
        }

        private static async Task InserirItemPedidoAsync(
            NpgsqlConnection connection,
            NpgsqlTransaction transaction,
            ItemPedido item)
        {
            var sql = @"
                INSERT INTO itempedido (
                    id_pedido,
                    codprod,
                    quantidade,
                    preco_unitario,
                    subtotal
                )
                VALUES (
                    @idPedido,
                    @codProd,
                    @quantidade,
                    @precoUnitario,
                    @subtotal
                )
                RETURNING id_item_pedido;
            ";

            await using var command = new NpgsqlCommand(sql, connection, transaction);

            command.Parameters.Add("@idPedido", NpgsqlDbType.Integer).Value = item.IdPedido;
            command.Parameters.Add("@codProd", NpgsqlDbType.Integer).Value = item.CodProd;
            command.Parameters.Add("@quantidade", NpgsqlDbType.Integer).Value = item.Quantidade;
            command.Parameters.Add("@precoUnitario", NpgsqlDbType.Numeric).Value = item.PrecoUnitario;
            command.Parameters.Add("@subtotal", NpgsqlDbType.Numeric).Value = item.Subtotal;

            var result = await command.ExecuteScalarAsync();

            if (result != null)
                item.Id = Convert.ToInt32(result);
        }

        private static async Task InserirPagamentoAsync(
            NpgsqlConnection connection,
            NpgsqlTransaction transaction,
            Pagamento pagamento)
        {
            var sql = @"
                INSERT INTO pagamento (
                    id_pedido,
                    forma_pagamento,
                    status_pagamento,
                    valor_pago,
                    data_hora_pagamento
                )
                VALUES (
                    @idPedido,
                    @formaPagamento,
                    @statusPagamento,
                    @valorPago,
                    @dataHoraPagamento
                )
                RETURNING id_pagamento;
            ";

            await using var command = new NpgsqlCommand(sql, connection, transaction);

            command.Parameters.Add("@idPedido", NpgsqlDbType.Integer).Value = pagamento.IdPedido;
            command.Parameters.Add("@formaPagamento", NpgsqlDbType.Varchar).Value = pagamento.FormaPagamento;
            command.Parameters.Add("@statusPagamento", NpgsqlDbType.Varchar).Value = pagamento.StatusPagamento;
            command.Parameters.Add("@valorPago", NpgsqlDbType.Numeric).Value = pagamento.ValorPago;
            command.Parameters.Add("@dataHoraPagamento", NpgsqlDbType.Timestamp).Value = pagamento.DataHoraPagamento;

            var result = await command.ExecuteScalarAsync();

            if (result != null)
                pagamento.Id = Convert.ToInt32(result);
        }

        private static async Task AtualizarEstoqueAsync(
            NpgsqlConnection connection,
            NpgsqlTransaction transaction,
            int codProd,
            int quantidade)
        {
            var sql = @"
                UPDATE produto
                SET estoque = estoque - @quantidade
                WHERE codprod = @codProd;
            ";

            await using var command = new NpgsqlCommand(sql, connection, transaction);

            command.Parameters.Add("@quantidade", NpgsqlDbType.Integer).Value = quantidade;
            command.Parameters.Add("@codProd", NpgsqlDbType.Integer).Value = codProd;

            await command.ExecuteNonQueryAsync();
        }

        private static Produto MapearProduto(NpgsqlDataReader reader)
        {
            return new Produto
            {
                Id = reader.GetInt32(reader.GetOrdinal("codprod")),
                Nome = reader.GetString(reader.GetOrdinal("nome_produto")),
                Preco = reader.GetDecimal(reader.GetOrdinal("preco")),
                Descricao = reader.IsDBNull(reader.GetOrdinal("descricao"))
                    ? null
                    : reader.GetString(reader.GetOrdinal("descricao")),
                Categoria = reader.IsDBNull(reader.GetOrdinal("categoria"))
                    ? null
                    : reader.GetString(reader.GetOrdinal("categoria")),
                Status = reader.IsDBNull(reader.GetOrdinal("status_disponibilidade"))
                    ? null
                    : reader.GetString(reader.GetOrdinal("status_disponibilidade")),
                Estoque = reader.IsDBNull(reader.GetOrdinal("estoque"))
                    ? null
                    : reader.GetInt32(reader.GetOrdinal("estoque")),
                ImagemUrl = reader.IsDBNull(reader.GetOrdinal("imagem_url"))
                    ? null
                    : reader.GetString(reader.GetOrdinal("imagem_url"))
            };
        }
    }
}