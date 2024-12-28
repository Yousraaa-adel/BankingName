using System;

namespace BankingSystem.Api.Entities;

public class Transaction
{
  public int Id { get; set; }

  public int TransactionTypeId { get; set; }
  public TransactionType? TransactionType { get; set; }

  public decimal Amount { get; set; }

  public int? AccountId { get; set; }
  public Account? Account { get; set; }

  public int? TargetAccountId { get; set; }
  public Account? TargetAccount { get; set; }

  public DateTime TransactionDate { get; set; }
}
