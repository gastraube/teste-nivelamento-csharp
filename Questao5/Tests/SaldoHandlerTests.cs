using FluentAssertions;
using MediatR;
using Moq;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Exceptions;
using Questao5.Application.Handlers;
using Questao5.Infrastructure.Database.Repositories;
using Xunit;

namespace Questao5.Tests
{
    public class SaldoHandlerTests
    {
        private Mock<ISaldoRepository>? saldorepository;
        private Mock<IContaRepository>? contarepoository;
        private Mock<IIdempotenciaRepository>? idempotenciaRepository;

        public void InitSaldoHandlerTests()
        {
            saldorepository = new Mock<ISaldoRepository>();
            contarepoository = new Mock<IContaRepository>();
            idempotenciaRepository = new Mock<IIdempotenciaRepository>();
        }

        [Fact]
        public async Task SaldoHandler_InvalidAccount()
        {
            InitSaldoHandlerTests();

            contarepoository.Setup(s => s.CheckValidAccount(It.IsAny<Guid>())).Returns(Task.FromResult(false));

            var saldoHandler = new SaldoHandler(saldorepository.Object, contarepoository.Object, idempotenciaRepository.Object);
            var command = new GetSaldoRequest() { ChaveIdempotencia = Guid.NewGuid(), IdConta = Guid.NewGuid() };

            await Assert.ThrowsAsync<INVALID_ACCOUNT>(() => saldoHandler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task SaldoHandler_InactiveAccount()
        {
            InitSaldoHandlerTests();

            contarepoository.Setup(s => s.CheckValidAccount(It.IsAny<Guid>())).Returns(Task.FromResult(true));
            contarepoository.Setup(s => s.CheckActiveAccount(It.IsAny<Guid>())).Returns(Task.FromResult(false));

            var saldoHandler = new SaldoHandler(saldorepository.Object, contarepoository.Object, idempotenciaRepository.Object);
            var command = new GetSaldoRequest() { ChaveIdempotencia = Guid.NewGuid(), IdConta = Guid.NewGuid() };

            await Assert.ThrowsAsync<INACTIVE_ACCOUNT>(() => saldoHandler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task SaldoHandler_IdempotencyAlreadyExists()
        {
            InitSaldoHandlerTests();

            idempotenciaRepository.Setup(s => s.ChekIdempotencia(It.IsAny<Guid>())).Returns(Task.FromResult(true));
            contarepoository.Setup(s => s.CheckValidAccount(It.IsAny<Guid>())).Returns(Task.FromResult(true));
            contarepoository.Setup(s => s.CheckActiveAccount(It.IsAny<Guid>())).Returns(Task.FromResult(true));

            var saldoHandler = new SaldoHandler(saldorepository.Object, contarepoository.Object, idempotenciaRepository.Object);
            var command = new GetSaldoRequest() { ChaveIdempotencia = Guid.NewGuid(), IdConta = Guid.NewGuid() };

            await Assert.ThrowsAsync<Exception>(() => saldoHandler.Handle(command, CancellationToken.None));
        }
    }
}
