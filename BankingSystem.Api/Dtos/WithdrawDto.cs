using System.ComponentModel.DataAnnotations;

namespace BankingSystem.Api.Dtos;

public record class WithdrawDto(
    [Required] int AccountId,
    [Required][Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero.")] decimal Amount
);
