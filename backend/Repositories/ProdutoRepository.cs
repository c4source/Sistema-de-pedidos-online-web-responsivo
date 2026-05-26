using Npgsql;
using NpgsqlTypes;
using Pim.Models;

namespace Pim.Repositories
{
    public class ProdutoRepository
    {
        private readonly string _connectionString;

        public ProdutoRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' nao encontrada.");
        }

        public async Task<List<Produto>> ListarAsync(string? categoria, bool somenteDisponiveis)
        {
            var produtos = new List<Produto>();

            var sql = @"
                SELECT codprod, nome_produto, preco, descricao, categoria, status_disponibilidade, estoque, imagem_url
                FROM produto
                WHERE (@categoria IS NULL OR LOWER(categoria) = LOWER(@categoria))
                  AND (@somenteDisponiveis = FALSE OR LOWER(status_disponibilidade) = 'disponivel')
                ORDER BY categoria, nome_produto;
            ";

            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            await using var command = new NpgsqlCommand(sql, connection);
            var categoriaParam = command.Parameters.Add("@categoria", NpgsqlDbType.Text);
            categoriaParam.Value = string.IsNullOrWhiteSpace(categoria) ? DBNull.Value : categoria;
            
           command.Parameters.Add("@somenteDisponiveis", NpgsqlDbType.Boolean).Value = somenteDisponiveis;

            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                produtos.Add(MapearProduto(reader));
            }

            return produtos;
        }

        public async Task<List<string>> ListarCategoriasAsync()
        {
            var categorias = new List<string>();

            var sql = @"
                SELECT DISTINCT categoria
                FROM produto
                WHERE categoria IS NOT NULL AND categoria <> ''
                ORDER BY categoria;
            ";

            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            await using var command = new NpgsqlCommand(sql, connection);
            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                categorias.Add(reader.GetString(0));
            }

            return categorias;
        }

        public async Task<Produto?> BuscarPorIdAsync(int id)
        {
            var sql = @"
                SELECT codprod, nome_produto, preco, descricao, categoria, status_disponibilidade, estoque, imagem_url
                FROM produto
                WHERE codprod = @id;
            ";

            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            await using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.AddWithValue("@id", id);

            await using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return MapearProduto(reader);
            }

            return null;
        }

        public async Task CriarAsync(Produto produto)
        {
            var sql = @"
                INSERT INTO produto (nome_produto, preco, descricao, categoria, status_disponibilidade, estoque, imagem_url)
                VALUES (@nome, @preco, @descricao, @categoria, @status, @estoque, @imagemUrl)
                RETURNING codprod;
            ";

            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            await using var command = new NpgsqlCommand(sql, connection);
            AdicionarParametrosProduto(command, produto);

            var novoId = await command.ExecuteScalarAsync();

            if (novoId != null)
                produto.Id = Convert.ToInt32(novoId);
        }

        public async Task AtualizarAsync(Produto produto)
        {
            var sql = @"
                UPDATE produto
                SET nome_produto = @nome,
                    preco = @preco,
                    descricao = @descricao,
                    categoria = @categoria,
                    status_disponibilidade = @status,
                    estoque = @estoque,
                    imagem_url = @imagemUrl
                WHERE codprod = @id;
            ";

            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            await using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.AddWithValue("@id", produto.Id);
            AdicionarParametrosProduto(command, produto);

            await command.ExecuteNonQueryAsync();
        }

        public async Task RemoverAsync(Produto produto)
        {
            var sql = @"
                DELETE FROM produto
                WHERE codprod = @id;
            ";

            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            await using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.AddWithValue("@id", produto.Id);

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

        private static void AdicionarParametrosProduto(NpgsqlCommand command, Produto produto)
        {
            command.Parameters.AddWithValue("@nome", produto.Nome);
            command.Parameters.AddWithValue("@preco", produto.Preco);
            command.Parameters.AddWithValue("@descricao", produto.Descricao ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@categoria", produto.Categoria ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@status", produto.Status ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@estoque", produto.Estoque ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@imagemUrl", produto.ImagemUrl ?? (object)DBNull.Value);
        }
    }
}