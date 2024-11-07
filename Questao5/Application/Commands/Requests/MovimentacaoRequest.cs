using MediatR;
using Questao5.Domain.Entities;

namespace Questao5.Application.Commands.Requests
{
    public class MovimentacaoRequest : IRequest
    {
        public Guid ChaveIdempotencia { get; set; }
        public Guid IdConta { get; set; }
        public double Valor { get; set; }
        public char TipoMovimentacao { get; set; }
    }
}
