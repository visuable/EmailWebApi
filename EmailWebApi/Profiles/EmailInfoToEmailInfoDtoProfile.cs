using AutoMapper;
using EmailWebApi.Db.Entities;
using EmailWebApi.Db.Entities.Dto;

namespace EmailWebApi.Profiles
{
    public class EmailInfoToEmailInfoDtoProfile : Profile
    {
        public EmailInfoToEmailInfoDtoProfile()
        {
            CreateMap<EmailInfo, EmailInfoDto>()
                .ForMember(x => x.Date, y => y.MapFrom(z => z.Date))
                .ForMember(x => x.UniversalId, y => y.MapFrom(z => z.UniversalId));
        }
    }
}