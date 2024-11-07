namespace Questao5.Infrastructure.Database.Repositories
{
    public interface IContaRepository
    {
        Task<bool> CheckValidAccount(Guid idConta);
        Task<bool> CheckActiveAccount(Guid idConta);
    }
}
