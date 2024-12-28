using System;
using BankingSystem.Api.Data;
using BankingSystem.Api.Dtos;
using BankingSystem.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace BankingSystem.Api.Services;

public class AccountService : IAccountService
{
  private readonly BankingSystemContext _DbContext;

  public AccountService(BankingSystemContext DbContext)
  {
    _DbContext = DbContext;
  }

  public async Task DepositAsync(int accountId, decimal amount)
  {
    var account = await _DbContext.Accounts.FindAsync(accountId);

    if (account is null)
      throw new Exception("Account not found");
    
    account.Balance =+ amount;

    var transactionType = await _DbContext.TransactionTypes
                          .Where(transactionType => transactionType.Name == "Deposit")
                          .Select(transactionType => transactionType.Id)
                          .FirstOrDefaultAsync();
    
    if (transactionType == 0)
      throw new Exception("Transaction type not found");

    var transaction = new Transaction
    {
      AccountId = accountId,
      TransactionTypeId = transactionType,
      Amount = amount,
      TransactionDate = DateTime.UtcNow
    };

    _DbContext.Transactions.Add(transaction);
    await _DbContext.SaveChangesAsync();
  }

  public async Task WithdrawAsync(int accountId, decimal amount)
  {
    var account = await _DbContext.Accounts.Include(acc => acc.AccountType)
                                        .FirstOrDefaultAsync(acc => acc.Id == accountId);

    if (account is null)
      throw new Exception("Account not found");

    // Get the TransactionTypeId dynamically (assuming "Withdraw" is the name for withdrawals)
    var transactionType = await _DbContext.TransactionTypes
                                          .FirstOrDefaultAsync(t => t.Name == "Withdraw");

    if (transactionType is null)
        throw new Exception("Transaction type for withdraw not found");

    // Set overdraft limit to 500 for Checking accounts if not set
    if (account.AccountType.Name == "Checking" && account.OverDraftLimit == null)
    {
        account.OverDraftLimit = 500;
    }

    // Save changes to the database
    await _DbContext.SaveChangesAsync();

    // For Checking accounts, check overdraft limit (500)
    if (account.AccountType.Name == "Checking")
    {
        if (amount > account.Balance + (account.OverDraftLimit ?? 0))
        {
            throw new Exception("Insufficient funds for this withdrawal including overdraft");
        }

    }
    else if (account.AccountType.Name == "Savings")
    {
        // For Savings accounts, just check the balance
        if (amount > account.Balance)
        {
            throw new Exception("Insufficient funds in Savings account");
        }
    }

    // Deduct the amount from the balance
    account.Balance -= amount;
    await _DbContext.SaveChangesAsync();

    // Create a new transaction record
    var transaction = new Transaction
    {
        TransactionTypeId = transactionType.Id,  // Use the dynamically fetched TransactionTypeId
        Amount = amount,
        AccountId = accountId,
        TransactionDate = DateTime.Now
    };

    // Add the transaction record to the DbContext and save changes
    await _DbContext.Transactions.AddAsync(transaction);
    await _DbContext.SaveChangesAsync();

  }

  public async Task TransferAsync(int sourceAccountId, int targetAccountId,
                                    decimal amount)
  {
    var SourceAccount = await _DbContext.Accounts.Include(acc => acc.AccountType)
                                                .FirstOrDefaultAsync(acc => acc.Id == sourceAccountId);

    var targetAccount = await _DbContext.Accounts.Include(acc => acc.AccountType)
                                              .FirstOrDefaultAsync(acc => acc.Id == targetAccountId);

    if (SourceAccount is null || targetAccount is null)
        throw new Exception("One or both accounts not found");

    // Get TransactionTypeIds for "Withdraw" and "Deposit"
    var withdrawTransactionType = await _DbContext.TransactionTypes
                                                  .FirstOrDefaultAsync(t => t.Name == "Withdraw");
    var depositTransactionType = await _DbContext.TransactionTypes
                                                  .FirstOrDefaultAsync(t => t.Name == "Deposit");

     // Check if the source account has sufficient balance (same logic as WithdrawAsync)
    if (SourceAccount.AccountType.Name == "Checking")
    {
        if (amount > SourceAccount.Balance + (SourceAccount.OverDraftLimit ?? 0))
            throw new Exception("Insufficient funds for the transfer");
    }
    else if (SourceAccount.AccountType.Name == "Savings")
    {
        if (amount > SourceAccount.Balance)
            throw new Exception("Insufficient funds in Savings account");
    }

    // Deduct the amount from the 'source' account
    SourceAccount.Balance -= amount;

    // Credit the 'to' account
    targetAccount.Balance += amount;

    // Create a transaction record for the 'source' account (withdrawal)
    var withdrawalTransaction = new Transaction
    {
        TransactionTypeId = withdrawTransactionType.Id,
        Amount = amount,
        AccountId = sourceAccountId,
        TransactionDate = DateTime.Now
    };

    // Create a transaction record for the 'to' account (deposit)
    var depositTransaction = new Transaction
    {
        TransactionTypeId = depositTransactionType.Id,
        Amount = amount,
        AccountId = targetAccountId,
        TransactionDate = DateTime.Now
    };

    // Save changes to the database
    await _DbContext.Transactions.AddAsync(withdrawalTransaction);
    await _DbContext.Transactions.AddAsync(depositTransaction);
    await _DbContext.SaveChangesAsync();
  }

  public async Task<decimal> GetBalanceAsync(int accountId)
  {
    var account = await _DbContext.Accounts
                        .FirstOrDefaultAsync(acc => acc.Id == accountId);

    if (account is null)
      throw new Exception("Account not found");

    return account.Balance;
  }

}
