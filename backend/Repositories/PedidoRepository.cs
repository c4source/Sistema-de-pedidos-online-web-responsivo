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
            const string sql = @"
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

        public async Task<List<Pedido>> ListarPedidosAsync()
        {
            const string sql = @"
                SELECT
                    p.id_pedido, p.codigo, p.nome_cliente, p.telefone_cliente,
                    p.tipo_entrega, p.rua_entrega, p.numero_entrega, p.bairro_entrega,
                    p.complemento_entrega, p.observacoes, p.data_hora_pedido,
                    p.status_pedido, p.valor_total,

                    pg.id_pagamento, pg.forma_pagamento, pg.status_pagamento,
                    pg.valor_pago, pg.data_hora_pagamento,

                    i.id_item_pedido, i.codprod, i.quantidade,
                    i.preco_unitario, i.subtotal,

                    pr.nome_produto, pr.preco, pr.descricao, pr.categoria,
                    pr.status_disponibilidade, pr.estoque, pr.imagem_url
                FROM pedido p
                LEFT JOIN pagamento pg ON pg.id_pedido = p.id_pedido
                LEFT JOIN itempedido i ON i.id_pedido = p.id_pedido
                LEFT JOIN produto pr ON pr.codprod = i.codprod
                ORDER BY p.data_hora_pedido DESC, p.id_pedido DESC;
            ";

            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            await using var command = new NpgsqlCommand(sql, connection);
            await using var reader = await command.ExecuteReaderAsync();

            return await MapearPedidosAsync(reader);
        }

        public async Task<Pedido?> BuscarPedidoCompletoAsync(int id)
        {
            const string sql = @"
                SELECT
                    p.id_pedido, p.codigo, p.nome_cliente, p.telefone_cliente,
                    p.tipo_entrega, p.rua_entrega, p.numero_entrega, p.bairro_entrega,
                    p.complemento_entrega, p.observacoes, p.data_hora_pedido,
                    p.status_pedido, p.valor_total,

                    pg.id_pagamento, pg.forma_pagamento, pg.status_pagamento,
                    pg.valor_pago, pg.data_hora_pagamento,

                    i.id_item_pedido, i.codprod, i.quantidade,
                    i.preco_unitario, i.subtotal,

                    pr.nome_produto, pr.preco, pr.descricao, pr.categoria,
                    pr.status_disponibilidade, pr.estoque, pr.imagem_url
                FROM pedido p
                LEFT JOIN pagamento pg ON pg.id_pedido = p.id_pedido
                LEFT JOIN itempedido i ON i.id_pedido = p.id_pedido
                LEFT JOIN produto pr ON pr.codprod = i.codprod
                WHERE p.id_pedido = @id
                ORDER BY i.id_item_pedido;
            ";

            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            await using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.Add("@id", NpgsqlDbType.Integer).Value = id;

            await using var reader = await command.ExecuteReaderAsync();

            var pedidos = await MapearPedidosAsync(reader);
            return pedidos.FirstOrDefault();
        }

        public async Task AtualizarStatusAsync(int id, string status)
        {
            const string sql = @"
                UPDATE pedido
                SET status_pedido = @status
                WHERE id_pedido = @id;
            ";

            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            await using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.Add("@id", NpgsqlDbType.Integer).Value = id;
            command.Parameters.Add("@status", NpgsqlDbType.Varchar).Value = status;

            await command.ExecuteNonQueryAsync();
        }

        public async Task CancelarPedidoAsync(Pedido pedido)
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            await using var transaction = await connection.BeginTransactionAsync();

            try
            {
                foreach (var item in pedido.Itens)
                {
                    await DevolverEstoqueAsync(connection, transaction, item.CodProd, item.Quantidade);
                }

                const string sqlPedido = @"
                    UPDATE pedido
                    SET status_pedido = 'cancelado'
                    WHERE id_pedido = @idPedido;
                ";

                await using (var commandPedido = new NpgsqlCommand(sqlPedido, connection, transaction))
                {
                    commandPedido.Parameters.Add("@idPedido", NpgsqlDbType.Integer).Value = pedido.Id;
                    await commandPedido.ExecuteNonQueryAsync();
                }

                const string sqlPagamento = @"
                    UPDATE pagamento
                    SET status_pagamento = 'cancelado'
                    WHERE id_pedido = @idPedido;
                ";

                await using (var commandPagamento = new NpgsqlCommand(sqlPagamento, connection, transaction))
                {
                    commandPagamento.Parameters.Add("@idPedido", NpgsqlDbType.Integer).Value = pedido.Id;
                    await commandPagamento.ExecuteNonQueryAsync();
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
            const string sql = @"
                INSERT INTO pedido (
                    codigo, nome_cliente, telefone_cliente, tipo_entrega,
                    rua_entrega, numero_entrega, bairro_entrega, complemento_entrega,
                    observacoes, data_hora_pedido, status_pedido, valor_total
                )
                VALUES (
                    @codigo, @nomeCliente, @telefoneCliente, @tipoEntrega,
                    @ruaEntrega, @numeroEntrega, @bairroEntrega, @complementoEntrega,
                    @observacoes, @dataHoraPedido, @statusPedido, @valorTotal
                )
                RETURNING id_pedido;
            ";

            await using var command = new NpgsqlCommand(sql, connection, transaction);

            command.Parameters.Add("@codigo", NpgsqlDbType.Varchar).Value = pedido.Codigo ?? (object)DBNull.Value;
            command.Parameters.Add("@nomeCliente", NpgsqlDbType.Varchar).Value = pedido.NomeCliente;
            command.Parameters.Add("@telefoneCliente", NpgsqlDbType.Varchar).Value = pedido.TelefoneCliente;
            command.Parameters.Add("@tipoEntrega", NpgsqlDbType.Varchar).Value = pedido.TipoEntrega;
            command.Parameters.Add("@ruaEntrega", NpgsqlDbType.Varchar).Value = pedido.RuaEntrega ?? (object)DBNull.Value;
            command.Parameters.Add("@numeroEntrega", NpgsqlDbType.Varchar).Value = pedido.NumeroEntrega ?? (object)DBNull.Value;
            command.Parameters.Add("@bairroEntrega", NpgsqlDbType.Varchar).Value = pedido.BairroEntrega ?? (object)DBNull.Value;
            command.Parameters.Add("@complementoEntrega", NpgsqlDbType.Varchar).Value = pedido.ComplementoEntrega ?? (object)DBNull.Value;
            command.Parameters.Add("@observacoes", NpgsqlDbType.Text).Value = pedido.Observacoes ?? (object)DBNull.Value;
            command.Parameters.Add("@dataHoraPedido", NpgsqlDbType.Timestamp).Value = pedido.DataHora;
            command.Parameters.Add("@statusPedido", NpgsqlDbType.Varchar).Value = pedido.Status;
            command.Parameters.Add("@valorTotal", NpgsqlDbType.Numeric).Value = pedido.ValorTotal;

            var result = await command.ExecuteScalarAsync();
            return Convert.ToInt32(result);
        }

        private static async Task InserirItemPedidoAsync(
            NpgsqlConnection connection,
            NpgsqlTransaction transaction,
            ItemPedido item)
        {
            const string sql = @"
                INSERT INTO itempedido (
                    id_pedido, codprod, quantidade, preco_unitario, subtotal
                )
                VALUES (
                    @idPedido, @codProd, @quantidade, @precoUnitario, @subtotal
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
            const string sql = @"
                INSERT INTO pagamento (
                    id_pedido, forma_pagamento, status_pagamento,
                    valor_pago, data_hora_pagamento
                )
                VALUES (
                    @idPedido, @formaPagamento, @statusPagamento,
                    @valorPago, @dataHoraPagamento
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
            const string sql = @"
                UPDATE produto
                SET estoque = estoque - @quantidade
                WHERE codprod = @codProd;
            ";

            await using var command = new NpgsqlCommand(sql, connection, transaction);

            command.Parameters.Add("@quantidade", NpgsqlDbType.Integer).Value = quantidade;
            command.Parameters.Add("@codProd", NpgsqlDbType.Integer).Value = codProd;

            await command.ExecuteNonQueryAsync();
        }

        private static async Task DevolverEstoqueAsync(
            NpgsqlConnection connection,
            NpgsqlTransaction transaction,
            int codProd,
            int quantidade)
        {
            const string sql = @"
                UPDATE produto
                SET estoque = estoque + @quantidade
                WHERE codprod = @codProd;
            ";

            await using var command = new NpgsqlCommand(sql, connection, transaction);

            command.Parameters.Add("@quantidade", NpgsqlDbType.Integer).Value = quantidade;
            command.Parameters.Add("@codProd", NpgsqlDbType.Integer).Value = codProd;

            await command.ExecuteNonQueryAsync();
        }

        private static async Task<List<Pedido>> MapearPedidosAsync(NpgsqlDataReader reader)
        {
            var pedidos = new Dictionary<int, Pedido>();

            while (await reader.ReadAsync())
            {
                var idPedido = reader.GetInt32(reader.GetOrdinal("id_pedido"));

                if (!pedidos.TryGetValue(idPedido, out var pedido))
                {
                    pedido = new Pedido
                    {
                        Id = idPedido,
                        Codigo = LerStringOuNull(reader, "codigo"),
                        NomeCliente = reader.GetString(reader.GetOrdinal("nome_cliente")),
                        TelefoneCliente = reader.GetString(reader.GetOrdinal("telefone_cliente")),
                        TipoEntrega = reader.GetString(reader.GetOrdinal("tipo_entrega")),
                        RuaEntrega = LerStringOuNull(reader, "rua_entrega"),
                        NumeroEntrega = LerStringOuNull(reader, "numero_entrega"),
                        BairroEntrega = LerStringOuNull(reader, "bairro_entrega"),
                        ComplementoEntrega = LerStringOuNull(reader, "complemento_entrega"),
                        Observacoes = LerStringOuNull(reader, "observacoes"),
                        DataHora = reader.GetDateTime(reader.GetOrdinal("data_hora_pedido")),
                        Status = reader.GetString(reader.GetOrdinal("status_pedido")),
                        ValorTotal = reader.GetDecimal(reader.GetOrdinal("valor_total")),
                        Itens = new List<ItemPedido>()
                    };

                    if (!reader.IsDBNull(reader.GetOrdinal("id_pagamento")))
                    {
                        pedido.Pagamento = new Pagamento
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("id_pagamento")),
                            IdPedido = idPedido,
                            FormaPagamento = reader.GetString(reader.GetOrdinal("forma_pagamento")),
                            StatusPagamento = reader.GetString(reader.GetOrdinal("status_pagamento")),
                            ValorPago = reader.GetDecimal(reader.GetOrdinal("valor_pago")),
                            DataHoraPagamento = reader.GetDateTime(reader.GetOrdinal("data_hora_pagamento"))
                        };
                    }

                    pedidos.Add(idPedido, pedido);
                }

                if (!reader.IsDBNull(reader.GetOrdinal("id_item_pedido")))
                {
                    var item = new ItemPedido
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("id_item_pedido")),
                        IdPedido = idPedido,
                        CodProd = reader.GetInt32(reader.GetOrdinal("codprod")),
                        Quantidade = reader.GetInt32(reader.GetOrdinal("quantidade")),
                        PrecoUnitario = reader.GetDecimal(reader.GetOrdinal("preco_unitario")),
                        Subtotal = reader.GetDecimal(reader.GetOrdinal("subtotal")),
                        Produto = new Produto
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("codprod")),
                            Nome = reader.GetString(reader.GetOrdinal("nome_produto")),
                            Preco = reader.GetDecimal(reader.GetOrdinal("preco")),
                            Descricao = LerStringOuNull(reader, "descricao"),
                            Categoria = LerStringOuNull(reader, "categoria"),
                            Status = LerStringOuNull(reader, "status_disponibilidade"),
                            Estoque = reader.IsDBNull(reader.GetOrdinal("estoque"))
                                ? null
                                : reader.GetInt32(reader.GetOrdinal("estoque")),
                            ImagemUrl = LerStringOuNull(reader, "imagem_url")
                        }
                    };

                    pedido.Itens.Add(item);
                }
            }

            return pedidos.Values.ToList();
        }

        private static Produto MapearProduto(NpgsqlDataReader reader)
        {
            return new Produto
            {
                Id = reader.GetInt32(reader.GetOrdinal("codprod")),
                Nome = reader.GetString(reader.GetOrdinal("nome_produto")),
                Preco = reader.GetDecimal(reader.GetOrdinal("preco")),
                Descricao = LerStringOuNull(reader, "descricao"),
                Categoria = LerStringOuNull(reader, "categoria"),
                Status = LerStringOuNull(reader, "status_disponibilidade"),
                Estoque = reader.IsDBNull(reader.GetOrdinal("estoque"))
                    ? null
                    : reader.GetInt32(reader.GetOrdinal("estoque")),
                ImagemUrl = LerStringOuNull(reader, "imagem_url")
            };
        }

        private static string? LerStringOuNull(NpgsqlDataReader reader, string coluna)
        {
            var ordinal = reader.GetOrdinal(coluna);
            return reader.IsDBNull(ordinal) ? null : reader.GetString(ordinal);
        }
    }
}