using System.ComponentModel.DataAnnotations;

namespace BankingSystem.Api.Dtos;

public record class AccountDto(
  [Required] int AccountTypeId,
  [Required] decimal Balance,
  decimal? OverDraftLimit
);
