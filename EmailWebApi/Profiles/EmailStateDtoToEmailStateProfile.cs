using AutoMapper;
using EmailWebApi.Entities;
using EmailWebApi.Entities.Dto;

namespace EmailWebApi.Profiles
{
    public class EmailStateToEmailStateDtoProfile : Profile
    {
        public EmailStateToEmailStateDtoProfile()
        {
            CreateMap<EmailState, EmailStateDto>()
                .ForMember(x => x.Status, y => y.MapFrom(z => z.Status));
        }
    }
}