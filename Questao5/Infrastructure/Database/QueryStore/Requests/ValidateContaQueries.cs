namespace Questao5.Infrastructure.Database.QueryStore.Requests
{
    public class ValidateContaQueries
    {
        public static string CheckValidAccountQuery(Guid idConta)
        {
            var query = @$"
                select count(*)
                from contacorrente cc            
                where cc.idcontacorrente = '{idConta.ToString().ToUpper()}'";

            return query;
        }

        public static string CheckActiveAccountQuery(Guid idConta)
        {
            var query = @$"
                select count(*)
                from contacorrente cc            
                where cc.idcontacorrente = '{idConta.ToString().ToUpper()}'
                and ativo = true";

            return query;
        }
    }
}
