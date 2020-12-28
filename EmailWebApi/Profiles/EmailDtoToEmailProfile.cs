using AutoMapper;
using EmailWebApi.Objects;
using EmailWebApi.Objects.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
