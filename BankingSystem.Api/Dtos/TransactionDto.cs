using System.ComponentModel.DataAnnotations;

namespace BankingSystem.Api.Dtos;

public class TransactionDto
{
  [Required] public int AccountNumber { get; set; }
  [Required] public decimal Amount { get; set; }
  [Required] public string TransactionTypeName { get; set; }
  public string TransactionDate { get; set; }

  // Parameterless constructor for AutoMapper
  public TransactionDto() { }

};
