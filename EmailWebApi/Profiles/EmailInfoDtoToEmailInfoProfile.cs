﻿using AutoMapper;
using EmailWebApi.Entities;
using EmailWebApi.Entities.Dto;

namespace EmailWebApi.Profiles
{
    public class EmailInfoDtoToEmailInfoProfile : Profile
    {
        public EmailInfoDtoToEmailInfoProfile()
        {
            CreateMap<EmailInfoDto, EmailInfo>()
                .ForMember(x => x.Date, y => y.MapFrom(z => z.Date))
                .ForMember(x => x.UniversalId, y => y.MapFrom(z => z.UniversalId));
        }
    }
}