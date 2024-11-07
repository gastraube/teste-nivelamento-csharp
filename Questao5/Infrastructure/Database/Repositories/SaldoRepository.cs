using Dapper;
using Microsoft.Data.Sqlite;
using Questao5.Application.Exceptions;
using Questao5.Domain.Entities;
using Questao5.Infrastructure.Database.QueryStore.Requests;
using Questao5.Infrastructure.Sqlite;

namespace Questao5.Infrastructure.Database.Repositories
{
    public class SaldoRepository : ISaldoRepository
    {
        private readonly DatabaseConfig _databaseConfig;

        public SaldoRepository(DatabaseConfig databaseConfig)
        {
            _databaseConfig = databaseConfig;
        }

        public async Task<GetSaldoResponse> GetSaldo(Guid idConta)
        {        
            using var connection = new SqliteConnection(_databaseConfig.Name);

            var saldo = await connection.QueryFirstOrDefaultAsync<GetSaldoResponse>(GetSaldoRequestQuery.Query(idConta));

            if (string.IsNullOrEmpty(saldo.IdConta))
                return new GetSaldoResponse() { IdConta = idConta.ToString().ToUpper(), ValorTotal = 0.0 };
            
            return saldo;
        }

    }
}
