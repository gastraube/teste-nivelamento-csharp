using Questao5.Application.Commands.Requests;

namespace Questao5.Infrastructure.Database.QueryStore.Requests
{
    public class MovimentacaoQuery
    {
        public static string Query(MovimentacaoRequest movimentacao)
        {
            var query = @$"INSERT INTO movimento(idmovimento, idcontacorrente, datamovimento, tipomovimento, valor) 
            VALUES('{Guid.NewGuid().ToString().ToUpper()}' ,'{movimentacao.IdConta.ToString().ToUpper()}', '{DateTime.Now.ToString("yyyy/MM/dd")}', '{movimentacao.TipoMovimentacao}', {movimentacao.Valor});";

            return query;
        }
    }
}
