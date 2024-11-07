using Questao5.Application.Commands.Requests;

namespace Questao5.Infrastructure.Database.Repositories
{
    public interface IMovimentacaoRepository
    {
        Task MovimentarConta(MovimentacaoRequest movimentacao);
    }
}
