using AutoMapper;
using EmailWebApi.Objects;
using EmailWebApi.Objects.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
