using AutoMapper;
using AutoMapper.Extensions.EnumMapping;
using EmailWebApi.Db.Entities;
using EmailWebApi.Db.Entities.Dto;

namespace EmailWebApi.Profiles
{
    public class EmailStatusToEmailStatusDtoProfile : Profile
    {
        public EmailStatusToEmailStatusDtoProfile()
        {
            CreateMap<EmailStatus, EmailStatusDto>().ConvertUsingEnumMapping(x => x.MapByName()).ReverseMap();
        }
    }
}