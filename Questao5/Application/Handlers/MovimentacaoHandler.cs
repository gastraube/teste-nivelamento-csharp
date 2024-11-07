using MediatR;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Exceptions;
using Questao5.Domain.Entities;
using Questao5.Infrastructure.Database.Repositories;

namespace Questao5.Application.Handlers
{
    public class MovimentacaoHandler : IRequestHandler<MovimentacaoRequest>
    {
        private readonly IMovimentacaoRepository _movimentacaoRepository;
        private readonly IContaRepository _contaRepository;
        private readonly IIdempotenciaRepository _idempotenciaRepository;

        public MovimentacaoHandler(IMovimentacaoRepository movimentacaoRepository, IContaRepository contaRepository, IIdempotenciaRepository idempotenciaRepository)
        {
            _movimentacaoRepository = movimentacaoRepository;
            _contaRepository = contaRepository;
            _idempotenciaRepository = idempotenciaRepository;
        }

        public async Task<Unit> Handle(MovimentacaoRequest request,
            CancellationToken cancellationToken)
        {
            if (await _idempotenciaRepository.ChekIdempotencia(request.ChaveIdempotencia))
                throw new Exception("Esta requisição já foi feita");

            if (!await _contaRepository.CheckValidAccount(request.IdConta))
               throw new INVALID_ACCOUNT();

            if (!await _contaRepository.CheckActiveAccount(request.IdConta))
                throw new INACTIVE_ACCOUNT();

            if (request.Valor < 0)
                throw new INVALID_VALUE();

            if (!TipoMovimentacao.Tipo.Contains(request.TipoMovimentacao))
                throw new INVALID_TYPE();

            await _movimentacaoRepository.MovimentarConta(request);

            await _idempotenciaRepository.InsertIdempotencia(request.ChaveIdempotencia, request, new object());

            return Unit.Value;
        }
    }
}
