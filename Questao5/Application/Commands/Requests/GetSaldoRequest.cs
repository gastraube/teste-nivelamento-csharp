using Castle.Core.Resource;
using MediatR;
using Questao5.Domain.Entities;

namespace Questao5.Application.Commands.Requests
{
    public class GetSaldoRequest : IRequest<GetSaldoResponse>
    {
        public Guid ChaveIdempotencia { get; set; }
        public Guid IdConta { get; set; }
    }
}
