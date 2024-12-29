using AutoMapper;
using BankingSystem.Api.Data;
using BankingSystem.Api.Dtos;
using BankingSystem.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace BankingSystem.Api.Services;

public class AccountService : IAccountService
{
  private readonly BankingSystemContext _DbContext;
  private readonly IMapper _mapper;

  public AccountService(BankingSystemContext DbContext, IMapper mapper)
  {
    _DbContext = DbContext;
    _mapper = mapper;
  }

  public async Task<Account> CreateAccountAsync(AccountDto accountDto)
  {
    // Validate account type exists
    var accountType = await _DbContext.AccountTypes
        .FirstOrDefaultAsync(a => a.Id == accountDto.AccountTypeId);

    if (accountType == null)
    {
      throw new ArgumentException($"Account Type with Id {accountDto.AccountTypeId} does not exist.");
    }

    // Create and save account
    var account = new Account
    {
      AccountTypeId = accountDto.AccountTypeId,
      Balance = accountDto.Balance,
      OverDraftLimit = accountDto.OverDraftLimit
    };

    _DbContext.Accounts.Add(account);
    await _DbContext.SaveChangesAsync();

    return account;
  }

  public async Task<TransactionDto> DepositAsync(int accountId, decimal amount)
  {
    var account = await _DbContext.Accounts.Include(acc => acc.AccountType)
                                          .Include(acc => acc.AccountType)
                                        .FirstOrDefaultAsync(acc => acc.Id == accountId);

    if (account is null)
      throw new Exception("Account not found");

    // Fixed annual interest rate
    const decimal fixedInterestRate = 0.18m; // 18%

    // Calculate interest if the account is a SavingsAccount
    if (account.AccountType.Name == "Savings")
    {
      var now = DateTime.Now;
      if (account.LastInterestCalculated.HasValue)
      {
        // Calculate elapsed time in years since the last interest calculation
        var elapsedTime = (now - account.LastInterestCalculated.Value).TotalDays / 365;
        if (elapsedTime > 0)
        {
          // Apply interest to the balance using the fixed rate
          var interest = account.Balance * fixedInterestRate * (decimal)elapsedTime;
          account.Balance += interest;
        }
      }

      // Update LastInterestCalculated to now
      account.LastInterestCalculated = now;
    }

    account.Balance += amount;

    var transactionType = await _DbContext.TransactionTypes
                                          .FirstOrDefaultAsync(t => t.Name == "Deposit");

    if (transactionType is null)
      throw new Exception("Transaction type not found");

    var transaction = new Transaction
    {
      AccountId = accountId,
      TransactionTypeId = transactionType.Id,
      Amount = amount,
      TransactionDate = DateTime.UtcNow
    };

    _DbContext.Transactions.Add(transaction);
    await _DbContext.SaveChangesAsync();

    // Map the Transaction entity to TransactionDto
    var transactionDto = _mapper.Map<TransactionDto>(transaction);
    transactionDto.TransactionDate = transaction.TransactionDate.ToString("dd-MM-yyyy HH:mm");  // Format the date
    transactionDto.Amount = $"${transaction.Amount:F2}"; // Format amount to include currency

    return transactionDto;
  }

  public async Task<TransactionDto> WithdrawAsync(int accountId, decimal amount)
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

    // Map the Transaction entity to TransactionDto
    var transactionDto = _mapper.Map<TransactionDto>(transaction);
    transactionDto.TransactionDate = transaction.TransactionDate.ToString("dd-MM-yyyy HH:mm");  // Format the date
    transactionDto.Amount = $"${transaction.Amount:F2}"; // Format amount to include currency

    return transactionDto;
  }

  public async Task<TransactionDto> TransferAsync(int sourceAccountId, int targetAccountId,
                                    decimal amount)
  {
    var SourceAccount = await _DbContext.Accounts.Include(acc => acc.AccountType)
                                                .FirstOrDefaultAsync(acc => acc.Id == sourceAccountId);

    var targetAccount = await _DbContext.Accounts.Include(acc => acc.AccountType)
                                              .FirstOrDefaultAsync(acc => acc.Id == targetAccountId);

    if (SourceAccount is null || targetAccount is null)
      throw new Exception("One or both accounts not found");

    // Get the Transfer TransactionType
    var transferTransactionType = await _DbContext.TransactionTypes
                                  .FirstOrDefaultAsync(t => t.Name == "Transfer");

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

    // Create a single transaction record for the transfer
    var transferTransaction = new Transaction
    {
      TransactionTypeId = transferTransactionType.Id,  // Use the 'Transfer' transaction type
      Amount = amount,
      AccountId = sourceAccountId,
      TargetAccountId = targetAccountId,  // Include the target account
      TransactionDate = DateTime.Now
    };

    // Save changes to the database
    _DbContext.Transactions.Add(transferTransaction);
    await _DbContext.SaveChangesAsync();

    // Map the Transaction entities to TransactionDto
    var transactionDto = _mapper.Map<TransactionDto>(transferTransaction);
    transactionDto.TransactionDate = transferTransaction.TransactionDate.ToString("dd-MM-yyyy HH:mm");  // Format the date
    transactionDto.Amount = $"${transferTransaction.Amount:F2}"; // Format amount to include currency

    return transactionDto;
  }

  public async Task<BalanceDto> GetBalanceAsync(int accountId)
  {
    var account = await _DbContext.Accounts
                        .FirstOrDefaultAsync(acc => acc.Id == accountId);

    if (account is null)
      throw new Exception("Account not found");

    // Map the Account entity to BalanceDto
    var balanceDto = new BalanceDto
    {
      AccountNumber = account.Id,
      Balance = $"${account.Balance:F2}" // Formats balance with currency and 2 decimal places
    };

    return balanceDto;
  }

}
