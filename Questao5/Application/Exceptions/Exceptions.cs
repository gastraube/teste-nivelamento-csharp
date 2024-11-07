using Newtonsoft.Json.Linq;
using System.Runtime.Serialization;

namespace Questao5.Application.Exceptions
{
    [Serializable]
    public class INVALID_ACCOUNT : Exception
    {
        public INVALID_ACCOUNT()
            : base("Apenas contas correntes cadastradas podem receber movimentação.")
        { }

    }

    [Serializable]
    public class INACTIVE_ACCOUNT : Exception
    {
        public INACTIVE_ACCOUNT()
            : base("Apenas contas correntes ativas podem consultar o saldo.")
        { }

    }

    [Serializable]
    public class INVALID_TYPE : Exception
    {
        public INVALID_TYPE()
            : base("Apenas os tipos “débito” ou “crédito” podem ser aceitos")
        { }
    }

    [Serializable]
    public class INVALID_VALUE : Exception
    {
        public INVALID_VALUE()
            : base("Apenas valores positivos podem ser recebidos")
        { }
    }

    
}
