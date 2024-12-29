using Microsoft.EntityFrameworkCore;
using AutoMapper;
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
    app.MapPost("/api/accounts/deposit", async (DepositDto depositDto, IAccountService accountService,
                                                  IMapper mapper) =>
    {
      // Validate the DTO
      if (!Validator.TryValidateObject(depositDto, new ValidationContext(depositDto), null, true))
      {
        return Results.BadRequest("Invalid input.");
      }

      try
      {
        // Call the service method for deposit
        var transactionDto = await accountService.DepositAsync(depositDto.AccountId, depositDto.Amount);

        return Results.Ok(new
        {
          Message = "Deposit successful.",
          Data = transactionDto
        });
      }
      catch (Exception ex)
      {
        // Return a structured error response when an exception is caught
        return Results.BadRequest(new { message = ex.Message });
      }
    });

    // Withdraw Endpoint
    app.MapPost("/api/accounts/withdraw", async (WithdrawDto withdrawDto, IAccountService accountService,
                                                  IMapper mapper) =>
    {
      // Validate the DTO
      if (!Validator.TryValidateObject(withdrawDto, new ValidationContext(withdrawDto), null, true))
      {
        return Results.BadRequest("Invalid input.");
      }

      try
      {
        // Call the service method for withdrawal
        var transactionDto = await accountService.WithdrawAsync(withdrawDto.AccountId, withdrawDto.Amount);

        return Results.Ok(new
        {
          Message = "Withdrawal successful.",
          Data = transactionDto
        });
      }
      catch (Exception ex)
      {
        // Return a structured error response when an exception is caught
        return Results.BadRequest(new { message = ex.Message });
      }
    });

    // Transfer Endpoint
    app.MapPost("/api/accounts/transfer", async (TransferDto transferDto, IAccountService accountService,
                                                    IMapper mapper) =>
    {
      if (!Validator.TryValidateObject(transferDto, new ValidationContext(transferDto), null, true))
      {
        return Results.BadRequest("Invalid input.");
      }

      try
      {
        var transactionDto = await accountService.TransferAsync(transferDto.AccountId, transferDto.TargetAccountId, transferDto.Amount);

        return Results.Ok(new
        {
          Message = "Transfer successful.",
          Data = transactionDto
        });
      }
      catch (Exception ex)
      {
        // Return a structured error response when an exception is caught
        return Results.BadRequest(new { message = ex.Message });
      }
    });

    // Get Balance Endpoint
    app.MapGet("/api/accounts/{id}/balance", async (int id, [FromServices] IAccountService accountService,
                                                  IMapper mapper) =>
    {
      try
      {
        // Call the service method for balance check
        var balanceDto = await accountService.GetBalanceAsync(id);

        if (balanceDto == null)
        {
          return Results.NotFound(new { Message = "Account not found." });
        }

        return Results.Ok(balanceDto);
      }
      catch (Exception ex)
      {
        return Results.BadRequest(new { Message = ex.Message });
      }
    });

  }
}
