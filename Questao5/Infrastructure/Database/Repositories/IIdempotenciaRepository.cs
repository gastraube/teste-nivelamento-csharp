namespace Questao5.Infrastructure.Database.Repositories
{
    public interface IIdempotenciaRepository
    {
        Task<bool> ChekIdempotencia(Guid ChaveIdempotencia);
        Task InsertIdempotencia<T, F>(Guid ChaveIdempotencia, T Request, F Response);
    }
}
