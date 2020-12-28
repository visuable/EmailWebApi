using AutoMapper;
using EmailWebApi.Objects;
using EmailWebApi.Objects.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
