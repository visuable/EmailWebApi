using AutoMapper;
using EmailWebApi.Db.Entities;
using EmailWebApi.Db.Entities.Dto;

namespace EmailWebApi.Profiles
{
    public class ApplicationStateToApplicationStateDtoProfile : Profile
    {
        public ApplicationStateToApplicationStateDtoProfile()
        {
            CreateMap<ApplicationState, ApplicationStateDto>()
                .ForMember(x => x.Sent, y => y.MapFrom(z => z.Sent))
                .ForMember(x => x.Error, y => y.MapFrom(z => z.Error))
                .ForMember(x => x.Query, y => y.MapFrom(z => z.Query))
                .ForMember(x => x.Total, y => y.MapFrom(z => z.Total));
        }
    }
}