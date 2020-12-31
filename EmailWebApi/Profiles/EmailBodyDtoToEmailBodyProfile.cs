using AutoMapper;
using EmailWebApi.Db.Entities;
using EmailWebApi.Db.Entities.Dto;

namespace EmailWebApi.Profiles
{
    public class EmailBodyDtoToEmailBodyProfile : Profile
    {
        public EmailBodyDtoToEmailBodyProfile()
        {
            CreateMap<EmailBodyDto, EmailBody>()
                .ForMember(x => x.Body, y => y.MapFrom(z => z.Body))
                .ForMember(x => x.Save, y => y.MapFrom(z => z.Save));
        }
    }
}