using AutoMapper;
using EmailWebApi.Models;
using EmailWebApi.Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmailWebApi.Helpers.Profiles
{
    public class EmailStatusToEmailStatusDtoProfile : Profile
    {
        public EmailStatusToEmailStatusDtoProfile()
        {
            CreateMap<EmailStatus, EmailStatusDto>().ForMember(x => x.IsArrived, y => y.MapFrom(z => z.IsArrived))
                .ForMember(x => x.EmailId, y => y.MapFrom(z => z.EmailId))
                .ForMember(x => x.SentDate, y => y.MapFrom(z => z.SentDate))
                .ForMember(x => x.SentTime, y => y.MapFrom(z => z.SentTime));
        }
    }
}
