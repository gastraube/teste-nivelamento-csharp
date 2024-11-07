using MediatR;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Exceptions;
using Questao5.Domain.Entities;
using Questao5.Infrastructure.Database.Repositories;

namespace Questao5.Application.Handlers
{
    public class SaldoHandler : IRequestHandler<GetSaldoRequest, GetSaldoResponse>
    {
        private readonly ISaldoRepository _saldoRepository;
        private readonly IContaRepository _contaRepository;
        private readonly IIdempotenciaRepository _idempotenciaRepository;

        public SaldoHandler(ISaldoRepository saldoRepository,
            IContaRepository contaRepository,
            IIdempotenciaRepository idempotenciaRepository)
        {
            _saldoRepository = saldoRepository;
            _contaRepository = contaRepository;
            _idempotenciaRepository = idempotenciaRepository;
        }

        public async Task<GetSaldoResponse> Handle(GetSaldoRequest request,
            CancellationToken cancellationToken)
        {
            if (await _idempotenciaRepository.ChekIdempotencia(request.ChaveIdempotencia))
                throw new Exception("Esta requisição já foi feita");

            if (!await _contaRepository.CheckValidAccount(request.IdConta))
                throw new INVALID_ACCOUNT();

            if (!await _contaRepository.CheckActiveAccount(request.IdConta))
                throw new INACTIVE_ACCOUNT();

            var response = await _saldoRepository.GetSaldo(request.IdConta);

            await _idempotenciaRepository.InsertIdempotencia(request.ChaveIdempotencia, request, response);

            return response;
        }
    }
}
