using AutoMapper;
using EmailWebApi.Objects;
using EmailWebApi.Objects.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmailWebApi.Profiles
{
    public class EmailBodyDtoToEmailBodyProfile : Profile
    {
        public EmailBodyDtoToEmailBodyProfile()
        {
            CreateMap<EmailBody, EmailBodyDto>()
                .ForMember(x => x.Body, y => y.MapFrom(z => z.Body))
                .ForMember(x => x.Save, y => y.MapFrom(z => z.Save));
        }
    }
}
