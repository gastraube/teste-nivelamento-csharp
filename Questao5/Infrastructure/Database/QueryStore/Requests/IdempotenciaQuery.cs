namespace Questao5.Infrastructure.Database.QueryStore.Requests
{
    public class IdempotenciaQuery
    {
        public static string CheckIdempotencia(Guid ChaveIdempotencia)
        {
            var query = @$"select count(*) from idempotencia
            where chave_idempotencia = '{ChaveIdempotencia.ToString().ToUpper()}'";

            return query;
        }

        public static string InsertIdempotencia(Guid ChaveIdempotencia, string request, string response)
        {
            var query = @$"
                insert into idempotencia(chave_idempotencia, requisicao, resultado)           
                values('{ChaveIdempotencia.ToString().ToUpper()}', '{request}', '{response}');";

            return query;
        }
    }
}
