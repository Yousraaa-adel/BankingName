using System.ComponentModel.DataAnnotations;

namespace BankingSystem.Api.Dtos;

public class BalanceDto
{
  [Required] public int AccountNumber { get; set; }
  [Required] public string Balance { get; set; }
}
