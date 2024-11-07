using Dapper;
using Microsoft.Data.Sqlite;
using Questao5.Domain.Entities;
using Questao5.Infrastructure.Database.QueryStore.Requests;
using Questao5.Infrastructure.Sqlite;

namespace Questao5.Infrastructure.Database.Repositories
{
    public class ContaRepository : IContaRepository
    {

        private readonly DatabaseConfig _databaseConfig;
        public ContaRepository(DatabaseConfig databaseConfig)
        {
            _databaseConfig = databaseConfig;
        }

        public async Task<bool> CheckValidAccount(Guid idConta)
        {
            using var connection = new SqliteConnection(_databaseConfig.Name);

            var result = await connection.QueryFirstOrDefaultAsync<int>(ValidateContaQueries.CheckValidAccountQuery(idConta));
           
            return (result > 0) ? true : false;
        }

        public async Task<bool> CheckActiveAccount(Guid idConta)
        {
            using var connection = new SqliteConnection(_databaseConfig.Name);

            var result = await connection.QueryFirstOrDefaultAsync<int>(ValidateContaQueries.CheckActiveAccountQuery(idConta));

            return (result > 0) ? true : false;
        }
    }
}
