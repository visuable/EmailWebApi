using AutoMapper;
using EmailWebApi.Models;
using EmailWebApi.Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmailWebApi.Helpers.Profiles
{
    public class EmailToEmailDtoProfile : Profile
    {
        public EmailToEmailDtoProfile()
        {
            CreateMap<Email, EmailDto>().ForMember(x => x.Adress, y => y.MapFrom(z => z.Adress))
                .ForMember(x => x.Body, y => y.MapFrom(z => z.Body))
                .ForMember(x => x.Title, y => y.MapFrom(z => z.Title))
                .ForMember(x => x.EmailStatus, y => y.MapFrom(z => z.EmailStatus));
        }
    }
}
