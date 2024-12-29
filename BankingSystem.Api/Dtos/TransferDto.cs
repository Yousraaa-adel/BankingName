using System.ComponentModel.DataAnnotations;

namespace BankingSystem.Api.Dtos;

public record class TransferDto(
    [Required] int AccountId,
    [Required] int TargetAccountId,
    [Required][Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero.")] decimal Amount
);
