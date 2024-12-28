using System.ComponentModel.DataAnnotations;

namespace BankingSystem.Api.Dtos;

public record class TransactionDto(
  [Required] int AccountId,
  [Required] int TransactionTypeId,
  int? TargetAccountId,
  [Required][Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero.")] decimal Amount,
  [Required] DateTime TransactionDate
);
