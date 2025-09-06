using System.ComponentModel;

namespace BankMore.ContaCorrente.Domain.Enums;


public enum TipoMovimentacao
{
    [Description("Crédito")]
    C = 1,

    [Description("Débito")]
    D = 2
}

