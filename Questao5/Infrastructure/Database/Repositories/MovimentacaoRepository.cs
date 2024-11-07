using Dapper;
using Microsoft.Data.Sqlite;
using Questao5.Application.Commands.Requests;
using Questao5.Infrastructure.Database.QueryStore.Requests;
using Questao5.Infrastructure.Sqlite;

namespace Questao5.Infrastructure.Database.Repositories
{
    public class MovimentacaoRepository : IMovimentacaoRepository
    {
        private readonly DatabaseConfig _databaseConfig;
        public MovimentacaoRepository(DatabaseConfig databaseConfig)
        {
            _databaseConfig = databaseConfig;
        }

        public async Task MovimentarConta(MovimentacaoRequest movimentacao)
        {
            using var connection = new SqliteConnection(_databaseConfig.Name);
            await connection.QueryAsync(MovimentacaoQuery.Query(movimentacao));
        }
    }
}
