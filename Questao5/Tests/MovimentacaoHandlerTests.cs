using Moq;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Exceptions;
using Questao5.Application.Handlers;
using Questao5.Infrastructure.Database.Repositories;
using Questao5.Infrastructure.Sqlite;
using Xunit;

namespace Questao5.Tests
{
    public class MovimentacaoHandlerTests
    {
        private Mock<IMovimentacaoRepository>? movimentacaoRepository;
        private Mock<IContaRepository>? contarepoository;
        private Mock<IIdempotenciaRepository>? idempotenciaRepository;

        public void InitMovimentacaoHandler()
        {
            movimentacaoRepository = new Mock<IMovimentacaoRepository>();
            contarepoository = new Mock<IContaRepository>();
            idempotenciaRepository = new Mock<IIdempotenciaRepository>();
        }

        [Fact]
        public async Task MovimentacaoHandler_InvalidAccount()
        {
            InitMovimentacaoHandler();

            contarepoository.Setup(s => s.CheckValidAccount(It.IsAny<Guid>())).Returns(Task.FromResult(false));

            var saldoHandler = new MovimentacaoHandler(movimentacaoRepository.Object, contarepoository.Object, idempotenciaRepository.Object);
            var command = new MovimentacaoRequest() { ChaveIdempotencia = Guid.NewGuid(), IdConta = Guid.NewGuid() };

            await Assert.ThrowsAsync<INVALID_ACCOUNT>(() => saldoHandler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task MovimentacaoHandler_InactiveAccount()
        {
            InitMovimentacaoHandler();

            contarepoository.Setup(s => s.CheckValidAccount(It.IsAny<Guid>())).Returns(Task.FromResult(true));
            contarepoository.Setup(s => s.CheckActiveAccount(It.IsAny<Guid>())).Returns(Task.FromResult(false));

            var saldoHandler = new MovimentacaoHandler(movimentacaoRepository.Object, contarepoository.Object, idempotenciaRepository.Object);
            var command = new MovimentacaoRequest() { ChaveIdempotencia = Guid.NewGuid(), IdConta = Guid.NewGuid() };

            await Assert.ThrowsAsync<INACTIVE_ACCOUNT>(() => saldoHandler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task MovimentacaoHandler_IdempotencyAlreadyExists()
        {
            InitMovimentacaoHandler();

            idempotenciaRepository.Setup(s => s.ChekIdempotencia(It.IsAny<Guid>())).Returns(Task.FromResult(true));
            contarepoository.Setup(s => s.CheckValidAccount(It.IsAny<Guid>())).Returns(Task.FromResult(true));
            contarepoository.Setup(s => s.CheckActiveAccount(It.IsAny<Guid>())).Returns(Task.FromResult(false));

            var saldoHandler = new MovimentacaoHandler(movimentacaoRepository.Object, contarepoository.Object, idempotenciaRepository.Object);
            var command = new MovimentacaoRequest() { ChaveIdempotencia = Guid.NewGuid(), IdConta = Guid.NewGuid() };

            await Assert.ThrowsAsync<Exception>(() => saldoHandler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task MovimentacaoHandler_NegativeValue()
        {
            InitMovimentacaoHandler();

            idempotenciaRepository.Setup(s => s.ChekIdempotencia(It.IsAny<Guid>())).Returns(Task.FromResult(false));
            contarepoository.Setup(s => s.CheckValidAccount(It.IsAny<Guid>())).Returns(Task.FromResult(true));
            contarepoository.Setup(s => s.CheckActiveAccount(It.IsAny<Guid>())).Returns(Task.FromResult(true));

            var saldoHandler = new MovimentacaoHandler(movimentacaoRepository.Object, contarepoository.Object, idempotenciaRepository.Object);
            var command = new MovimentacaoRequest() { ChaveIdempotencia = Guid.NewGuid(), IdConta = Guid.NewGuid(), Valor = -1 };

            await Assert.ThrowsAsync<INVALID_VALUE>(() => saldoHandler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task MovimentacaoHandler_InvalidType()
        {
            InitMovimentacaoHandler();

            idempotenciaRepository.Setup(s => s.ChekIdempotencia(It.IsAny<Guid>())).Returns(Task.FromResult(false));
            contarepoository.Setup(s => s.CheckValidAccount(It.IsAny<Guid>())).Returns(Task.FromResult(true));
            contarepoository.Setup(s => s.CheckActiveAccount(It.IsAny<Guid>())).Returns(Task.FromResult(true));

            var saldoHandler = new MovimentacaoHandler(movimentacaoRepository.Object, contarepoository.Object, idempotenciaRepository.Object);
            var command = new MovimentacaoRequest() { ChaveIdempotencia = Guid.NewGuid(),
                IdConta = Guid.NewGuid(), Valor = 1, TipoMovimentacao = 'Z' };

            await Assert.ThrowsAsync<INVALID_TYPE>(() => saldoHandler.Handle(command, CancellationToken.None));
        }

    }
}
