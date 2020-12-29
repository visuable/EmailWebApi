using AutoMapper;
using EmailWebApi.Entities;
using EmailWebApi.Entities.Dto;

namespace EmailWebApi.Profiles
{
    public class EmailStateDtoToEmailStateProfile : Profile
    {
        public EmailStateDtoToEmailStateProfile()
        {
            CreateMap<EmailStateDto, EmailState>()
                .ForMember(x => x.Status, y => y.MapFrom(z => z.Status));
        }
    }
}