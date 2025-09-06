﻿namespace BankMore.ContaCorrente.Domain.Entities;

public class Idempotencia
{
    public string ChaveIdempotencia { get; set; } = Guid.NewGuid().ToString();
    public string Requisicao { get; set; } = string.Empty;
    public string Resultado { get;  set; } = string.Empty;
        
}
