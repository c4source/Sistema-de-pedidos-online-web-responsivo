using Npgsql;
using NpgsqlTypes;
using Pim.Models;

namespace Pim.Repositories
{
    public class ColaboradorRepository
    {
        private readonly string _connectionString;

        public ColaboradorRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' nao encontrada.");
        }

        public async Task<List<Colaborador>> ListarAsync()
        {
            var colaboradores = new List<Colaborador>();

            const string sql = @"
                SELECT id_colaborador, nome_usuario, email_usuario, senha_usuario
                FROM colaborador
                ORDER BY id_colaborador;
            ";

            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            await using var command = new NpgsqlCommand(sql, connection);
            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                colaboradores.Add(MapearColaborador(reader));
            }

            return colaboradores;
        }

        public async Task<Colaborador?> BuscarPorIdAsync(int id)
        {
            const string sql = @"
                SELECT id_colaborador, nome_usuario, email_usuario, senha_usuario
                FROM colaborador
                WHERE id_colaborador = @id;
            ";

            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            await using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.Add("@id", NpgsqlDbType.Integer).Value = id;

            await using var reader = await command.ExecuteReaderAsync();

            if (!await reader.ReadAsync())
                return null;

            return MapearColaborador(reader);
        }

        public async Task<bool> EmailExisteAsync(string email)
        {
            const string sql = @"
                SELECT COUNT(1)
                FROM colaborador
                WHERE email_usuario = @email;
            ";

            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            await using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.Add("@email", NpgsqlDbType.Varchar).Value = email;

            var result = await command.ExecuteScalarAsync();

            return Convert.ToInt32(result) > 0;
        }

        public async Task CriarAsync(Colaborador colaborador)
        {
            const string sql = @"
                INSERT INTO colaborador (nome_usuario, email_usuario, senha_usuario)
                VALUES (@nome, @email, @senha)
                RETURNING id_colaborador;
            ";

            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            await using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.Add("@nome", NpgsqlDbType.Varchar).Value = colaborador.Nome;
            command.Parameters.Add("@email", NpgsqlDbType.Varchar).Value = colaborador.Email;
            command.Parameters.Add("@senha", NpgsqlDbType.Varchar).Value = colaborador.Senha;

            var id = await command.ExecuteScalarAsync();

            if (id != null)
                colaborador.Id = Convert.ToInt32(id);
        }

        public async Task AtualizarAsync(Colaborador colaborador)
        {
            const string sql = @"
                UPDATE colaborador
                SET nome_usuario = @nome,
                    email_usuario = @email
                WHERE id_colaborador = @id;
            ";

            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            await using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.Add("@id", NpgsqlDbType.Integer).Value = colaborador.Id;
            command.Parameters.Add("@nome", NpgsqlDbType.Varchar).Value = colaborador.Nome;
            command.Parameters.Add("@email", NpgsqlDbType.Varchar).Value = colaborador.Email;

            await command.ExecuteNonQueryAsync();
        }

        public async Task RemoverAsync(int id)
        {
            const string sql = @"
                DELETE FROM colaborador
                WHERE id_colaborador = @id;
            ";

            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            await using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.Add("@id", NpgsqlDbType.Integer).Value = id;

            await command.ExecuteNonQueryAsync();
        }

        private static Colaborador MapearColaborador(NpgsqlDataReader reader)
        {
            return new Colaborador
            {
                Id = reader.GetInt32(reader.GetOrdinal("id_colaborador")),
                Nome = reader.GetString(reader.GetOrdinal("nome_usuario")),
                Email = reader.GetString(reader.GetOrdinal("email_usuario")),
                Senha = reader.GetString(reader.GetOrdinal("senha_usuario"))
            };
        }
    }
}