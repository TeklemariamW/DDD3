using AutoMapper;
using Entities.DTOs.Account;
using Entities.DTOs.Owner;
using Entities.Models;

namespace API.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Owner, OwnerDto>();
        CreateMap<OwnerForCreationDto, Owner>();
        CreateMap<OwnerForUpdateDto, Owner>();

        CreateMap<Account, AccountDto>();
        CreateMap<AccountForCreationDto, Account>();
    }
}
