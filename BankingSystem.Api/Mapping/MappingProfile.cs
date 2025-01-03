using AutoMapper;
using BankingSystem.Api.Dtos;
using BankingSystem.Api.Entities;

namespace BankingSystem.Api.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Map DepositDto to Transaction entity for depositing
            CreateMap<DepositDto, Transaction>()
                .ForMember(dest => dest.TransactionTypeId, opt => opt.MapFrom(src => 1))  // Assuming 1 represents Deposit in TransactionType
                .ForMember(dest => dest.TransactionDate, opt => opt.MapFrom(src => DateTime.Now))  // Set transaction date as now
                .ReverseMap();  // Reverse mapping to handle updates

            // Map WithdrawDto to Transaction entity for withdrawing
            CreateMap<WithdrawDto, Transaction>()
                  .ForMember(dest => dest.TransactionTypeId, opt => opt.MapFrom(src => 2))  // Assuming 2 represents Withdrawal in TransactionType
                  .ForMember(dest => dest.TransactionDate, opt => opt.MapFrom(src => DateTime.Now))  // Set transaction date as now
                  .ReverseMap();  // Reverse mapping to handle updates

            // Map TransferDto to Transaction entity for transferring
            CreateMap<TransferDto, Transaction>()
                .ForMember(dest => dest.TransactionTypeId, opt => opt.MapFrom(src => 3))  // Assuming 3 represents Transfer in TransactionType
                .ForMember(dest => dest.TransactionDate, opt => opt.MapFrom(src => DateTime.Now))  // Set transaction date as now
                .ForMember(dest => dest.TargetAccountId, opt => opt.MapFrom(src => src.TargetAccountId))  // Map Target Account
                .ReverseMap();  // Reverse mapping to handle updates

            // Map GetBalanceDto to Account entity to fetch the balance
            CreateMap<GetBalanceDto, Account>()
                .ReverseMap();  // Reverse mapping is not needed here, it's just used for fetching

            // Map AccountDto to Account entity for account creation
            CreateMap<AccountDto, Account>()
                .ForMember(dest => dest.Balance, opt => opt.MapFrom(src => src.Balance))  // Map Balance
                .ForMember(dest => dest.OverDraftLimit, opt => opt.MapFrom(src => src.OverDraftLimit))  // Map OverDraftLimit
                .ReverseMap();  // Reverse mapping for updating accounts if needed

            // Map Transaction entity to TransactionDto
            CreateMap<Transaction, TransactionDto>()
                .ForMember(dest => dest.AccountNumber, opt => opt.MapFrom(src => src.AccountId))  // Assuming AccountId maps to AccountNumber
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))  // Map Amount
                .ForMember(dest => dest.TransactionDate, opt => opt.MapFrom(src => src.TransactionDate))  // Map TransactionDate
                .ForMember(dest => dest.TransactionTypeName, opt => opt.MapFrom(src => src.TransactionType.Name))
                .ForMember(dest => dest.TargetAccountId, opt => opt.MapFrom(src => src.TargetAccountId))
                .ReverseMap();  // Allow reverse mapping if needed

            CreateMap<Account, BalanceDto>()
            .ForMember(dest => dest.AccountNumber, opt => opt.MapFrom(src => src.Id)) // Map Id to AccountNumber
            .ForMember(dest => dest.Balance, opt => opt.MapFrom(src => src.Balance));
        }
    }
}
