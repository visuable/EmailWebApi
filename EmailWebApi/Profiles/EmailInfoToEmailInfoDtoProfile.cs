using AutoMapper;
using EmailWebApi.Objects;
using EmailWebApi.Objects.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmailWebApi.Profiles
{
    public class EmailInfoToEmailInfoDtoProfile : Profile
    {
        public EmailInfoToEmailInfoDtoProfile()
        {
            CreateMap<EmailInfo, EmailInfoDto>()
                .ForMember(x => x.Date, y => y.MapFrom(z => z.Date))
                .ForMember(x => x.UniversalId, y => y.MapFrom(z => z.UniversalId));
        }
    }
}
