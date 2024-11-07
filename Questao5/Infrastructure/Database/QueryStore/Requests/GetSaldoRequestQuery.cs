using Questao5.Domain.Entities;

namespace Questao5.Infrastructure.Database.QueryStore.Requests
{
    public class GetSaldoRequestQuery
    {
        public static string Query(Guid idConta)
        {
            var query = @$"select 
            sum(valor) as 'ValorTotal',
            m.idcontacorrente as 'IdConta'
            from movimento m
            where m.idcontacorrente = '{idConta.ToString().ToUpper()}'
            group by m.idcontacorrente";

            return query;
        }

    }
}
