using System;

namespace BankingSystem.Api.Services;

public interface IAccountService
{
  Task DepositAsync(int accountId, decimal amount);
  Task WithdrawAsync(int accountId, decimal amount);
  Task TransferAsync(int sourceAccountId, int targetAccountId, decimal amount);
  Task<decimal> GetBalanceAsync(int accountId);
}
