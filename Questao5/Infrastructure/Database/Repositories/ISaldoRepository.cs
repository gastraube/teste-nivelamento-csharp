using Questao5.Domain.Entities;

namespace Questao5.Infrastructure.Database.Repositories
{
    public interface ISaldoRepository
    {
        Task<GetSaldoResponse> GetSaldo(Guid idConta);        
    }
}
