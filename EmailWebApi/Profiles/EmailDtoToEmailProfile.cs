using AutoMapper;
using EmailWebApi.Db.Entities;
using EmailWebApi.Db.Entities.Dto;

namespace EmailWebApi.Profiles
{
    public class EmailDtoToEmailProfile : Profile
    {
        public EmailDtoToEmailProfile()
        {
            CreateMap<EmailDto, Email>()
                .ForMember(x => x.Content, y => y.MapFrom(z => z.Content));
        }
    }
}