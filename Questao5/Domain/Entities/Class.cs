namespace Questao5.Domain.Entities
{
    public static class TipoMovimentacao
    {
        public static IEnumerable<char> Tipo { get; set; } = new List<char>() { 'C', 'D'};
    }
}
