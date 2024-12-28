using System;

namespace BankingSystem.Api.Entities;

public class AccountType
{
  public int Id{ get; set; }

  public required string Name{ get; set; }

  public decimal InterestRate { get; set; }
}
