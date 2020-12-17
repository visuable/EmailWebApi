using AutoMapper;
using EmailWebApi.Models;
using EmailWebApi.Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmailWebApi.Helpers.Profiles
{
    public class StatusModelToStatusModelDtoProfile : Profile
    {
        public StatusModelToStatusModelDtoProfile()
        {
            CreateMap<StatusModel, StatusModelDto>().ForMember(x => x.Failed, y => y.MapFrom(z => z.Failed))
                .ForMember(x => x.Query, y => y.MapFrom(z => z.Query))
                .ForMember(x => x.Sent, y => y.MapFrom(z => z.Sent))
                .ForMember(x => x.Total, y => y.MapFrom(z => z.Total));
        }
    }
}
