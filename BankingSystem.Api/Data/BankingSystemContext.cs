using System;
using BankingSystem.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace BankingSystem.Api.Data;

public class BankingSystemContext(DbContextOptions<BankingSystemContext> options)
    : DbContext(options)
{
    public DbSet<Account> Accounts => Set<Account>();
    public DbSet<AccountType> AccountTypes => Set<AccountType>();

    public DbSet<Transaction> Transactions => Set<Transaction>();

    public DbSet<TransactionType> TransactionTypes => Set<TransactionType>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AccountType>().HasData(
            new AccountType { Id = 1, Name = "Checking" },
            new AccountType { Id = 2, Name = "Savings", InterestRate = 0.02M }
        );

        modelBuilder.Entity<TransactionType>().HasData(
            new TransactionType { Id = 1, Name = "Deposit" },
            new TransactionType { Id = 2, Name = "Withdraw" },
            new TransactionType { Id = 3, Name = "Transfer" },
            new TransactionType { Id = 4, Name = "Balance Check" }
        );
    }
}
