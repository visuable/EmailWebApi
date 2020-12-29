using AutoMapper;
using EmailWebApi.Entities;
using EmailWebApi.Entities.Dto;

namespace EmailWebApi.Profiles
{
    public class EmailContentDtoToEmailContentProfile : Profile
    {
        public EmailContentDtoToEmailContentProfile()
        {
            CreateMap<EmailContentDto, EmailContent>()
                .ForMember(x => x.Address, y => y.MapFrom(z => z.Address))
                .ForMember(x => x.Body, y => y.MapFrom(z => z.Body))
                .ForMember(x => x.Title, y => y.MapFrom(z => z.Title));
        }
    }
}