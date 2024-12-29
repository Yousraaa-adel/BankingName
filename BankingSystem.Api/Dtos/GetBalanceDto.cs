using System.ComponentModel.DataAnnotations;

namespace BankingSystem.Api.Dtos;

public record class GetBalanceDto(
  [Required] int AccountId
);
