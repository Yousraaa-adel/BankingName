using System;

namespace BankingSystem.Api.Entities;

public class Account
{
  public int Id { get; set; }

  public int AccountTypeId { get; set; }
  public AccountType? AccountType { get; set; }

  public DateTime? LastInterestCalculated { get; set; }

  public decimal Balance { get; set; }

  public decimal? OverDraftLimit { get; set; }
}
