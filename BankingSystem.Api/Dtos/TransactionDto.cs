using System.ComponentModel.DataAnnotations;

namespace BankingSystem.Api.Dtos;

public class TransactionDto
{
  [Required] public int AccountNumber { get; set; }
  [Required] public string Amount { get; set; }
  [Required] public string TransactionTypeName { get; set; }
  public int? TargetAccountId { get; set; }
  public string TransactionDate { get; set; }

  // Parameterless constructor for AutoMapper
  public TransactionDto() { }

};
