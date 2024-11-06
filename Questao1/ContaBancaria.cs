using System;
using System.Globalization;

namespace Questao1
{
    public class ContaBancaria
    {
        public int Numero { get; }
        private string _nomeTitular;
        public string NomeTitular
        {
            get => _nomeTitular;
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _nomeTitular = value;
                }
            }
        }

        public double Saldo { get; private set; }

        private const double TaxaSaque = 3.50;

        public ContaBancaria(int numero, string nomeTitular, double depositoInicial = 0)
        {
            Numero = numero;
            NomeTitular = nomeTitular;
            Saldo = depositoInicial;
        }

        public void Deposito(double valor)
        {
            if (valor > 0)
            {
                Saldo += valor;
            }
            else
            {
                throw new ArgumentException("O valor do depósito deve ser positivo.");
            }
        }

        public void Saque(double valor)
        {
            if (valor > 0)
            {
                Saldo -= (valor + TaxaSaque);
            }
            else
            {
                throw new ArgumentException("O valor do saque deve ser positivo.");
            }
        }

        public override string ToString()
        {
            return $"Conta {Numero}, Titular: {NomeTitular}, Saldo: $ {Saldo}";
        }
    }
}
