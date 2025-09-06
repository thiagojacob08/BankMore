using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankMore.ContaCorrente.Application.DTOs;

public class ConsultarSaldoDto
{
    public int NumeroConta { get; set; }
    public string Nome { get; set; } = string.Empty;
    public DateTime DataHoraConsulta { get; set; }
    public decimal Saldo { get; set; }
}
