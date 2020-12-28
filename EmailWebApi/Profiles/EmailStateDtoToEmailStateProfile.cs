using AutoMapper;
using EmailWebApi.Objects;
using EmailWebApi.Objects.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
