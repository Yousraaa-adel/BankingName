using AutoMapper;
using BankingSystem.Api.Dtos;
using BankingSystem.Api.Entities;

namespace BankingSystem.Api.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
          // Map Account entity to AccountDto
          CreateMap<Account, AccountDto>()
            .ReverseMap();  // Add reverse mapping

          // Map Transaction entity to TransactionDto
          CreateMap<Transaction, TransactionDto>()
                .ForMember(dest => dest.TargetAccountId, opt => opt.MapFrom(src => src.TargetAccount.Id))
                .ReverseMap();  // Add reverse mapping
        }
    }
}
