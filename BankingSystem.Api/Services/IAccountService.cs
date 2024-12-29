using System;
using BankingSystem.Api.Dtos;
using BankingSystem.Api.Entities;

namespace BankingSystem.Api.Services;

public interface IAccountService
{
  Task<TransactionDto> DepositAsync(int accountId, decimal amount);
  Task<TransactionDto> WithdrawAsync(int accountId, decimal amount);
  Task<TransactionDto> TransferAsync(int sourceAccountId, int targetAccountId, decimal amount);
  Task<decimal> GetBalanceAsync(int accountId);
  Task<Account> CreateAccountAsync(AccountDto accountDto);
}
