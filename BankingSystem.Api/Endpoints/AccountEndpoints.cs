using Microsoft.EntityFrameworkCore;
using BankingSystem.Api.Data;
using BankingSystem.Api.Dtos;
using BankingSystem.Api.Services;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BankingSystem.Api.Endpoints;

public static class AccountEndpoints
{
  public static void MapAccountEndpoints(this WebApplication app)
  {

    app.MapPost("/api/accounts", async ([FromBody] AccountDto accountDto, IAccountService accountService) =>
    {
      // Validate the DTO
      if (!Validator.TryValidateObject(accountDto, new ValidationContext(accountDto), null, true))
      {
        return Results.BadRequest("Invalid input.");
      }

      // Call the service method to create the account
      var account = await accountService.CreateAccountAsync(accountDto);

      // Return the created account as a response
      return Results.Ok(account);
    });



    // Deposit Endpoint
    app.MapPost("/api/accounts/deposit", async (DepositDto depositDto, IAccountService accountService) =>
    {
      // Validate the DTO
      if (!Validator.TryValidateObject(depositDto, new ValidationContext(depositDto), null, true))
      {
        return Results.BadRequest("Invalid input.");
      }

      // Call the service method for deposit
      await accountService.DepositAsync(depositDto.AccountId, depositDto.Amount);

      return Results.Ok("Deposit successful.");
    });

    // Withdraw Endpoint
    app.MapPost("/api/accounts/withdraw", async (WithdrawDto withdrawDto, IAccountService accountService) =>
    {
      // Validate the DTO
      if (!Validator.TryValidateObject(withdrawDto, new ValidationContext(withdrawDto), null, true))
      {
        return Results.BadRequest("Invalid input.");
      }

      // Call the service method for withdrawal
      await accountService.WithdrawAsync(withdrawDto.AccountId, withdrawDto.Amount);

      return Results.Ok("Withdrawal successful.");
    });

    // Transfer Endpoint
    app.MapPost("/api/accounts/transfer", async (TransferDto transferDto, IAccountService accountService) =>
    {
      if (!Validator.TryValidateObject(transferDto, new ValidationContext(transferDto), null, true))
      {
        return Results.BadRequest("Invalid input.");
      }

      await accountService.TransferAsync(transferDto.AccountId, transferDto.TargetAccountId, transferDto.Amount);
      return Results.Ok(new { Message = "Transfer successful.", Data = transferDto });
    });

    // Get Balance Endpoint
    app.MapGet("/api/accounts/balance", async ([FromBody] GetBalanceDto getBalanceDto, [FromServices] IAccountService accountService) =>
    {
      if (!Validator.TryValidateObject(getBalanceDto, new ValidationContext(getBalanceDto), null, true))
      {
        return Results.BadRequest("Invalid input.");
      }

      // Call the service method for balance check
      var balance = await accountService.GetBalanceAsync(getBalanceDto.AccountId);

      if (balance == null)
      {
        return Results.NotFound("Account not found.");
      }

      return Results.Ok(new { Balance = balance });
    });

  }
}
