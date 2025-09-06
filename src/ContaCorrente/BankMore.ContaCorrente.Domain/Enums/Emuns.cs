using System.ComponentModel;

namespace BankMore.ContaCorrente.Domain.Enums
{
    public class Emuns
    {
        public enum TipoMovimentacao
        {
            [Description("Crédito")]
            C = 1,

            [Description("Débito")]
            D = 2
        }
    }
}
