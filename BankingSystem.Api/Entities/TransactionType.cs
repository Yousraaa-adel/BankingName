using System;

namespace BankingSystem.Api.Entities;

public class TransactionType
{
  public int Id { get; set; }
  public required string Name { get; set; }
}
