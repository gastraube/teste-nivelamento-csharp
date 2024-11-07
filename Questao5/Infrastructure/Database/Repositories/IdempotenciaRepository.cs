using Dapper;
using Microsoft.Data.Sqlite;
using Newtonsoft.Json;
using Questao5.Infrastructure.Database.QueryStore.Requests;
using Questao5.Infrastructure.Sqlite;

namespace Questao5.Infrastructure.Database.Repositories
{
    public class IdempotenciaRepository : IIdempotenciaRepository
    {
        private readonly DatabaseConfig _databaseConfig;
        public IdempotenciaRepository(DatabaseConfig databaseConfig)
        {
            _databaseConfig = databaseConfig;
        }

        public async Task<bool> ChekIdempotencia(Guid ChaveIdempotencia)
        {
            using var connection = new SqliteConnection(_databaseConfig.Name);

            var result = await connection.QueryFirstOrDefaultAsync<int>(IdempotenciaQuery.CheckIdempotencia(ChaveIdempotencia));

            return (result > 0) ? true : false;
        }

        public async Task InsertIdempotencia<T, F>(Guid ChaveIdempotencia, T Request, F Response)
        {
            var request = JsonConvert.SerializeObject(Request);
            var response = JsonConvert.SerializeObject(Response);

            using var connection = new SqliteConnection(_databaseConfig.Name);

            await connection.QueryFirstOrDefaultAsync<int>(IdempotenciaQuery.InsertIdempotencia(ChaveIdempotencia ,request, response));
        }
    }
}
