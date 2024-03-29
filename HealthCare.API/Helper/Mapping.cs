using AutoMapper;
using HealthCare.Core.Entities;
using HealthCare.Core.Entities.Data;
using HealthCare.PL.DTOs;

namespace HealthCare.PL.Helper
{
    public class Mapping : Profile
    {

        public Mapping()
        {
            CreateMap<Services, ServiceToReturnDto>()
                .ForMember(Sr => Sr.Street, O => O.MapFrom(S => S.Address.Street))
                .ForMember(Sr => Sr.Region, O => O.MapFrom(S => S.Address.Region))
                .ForMember(Sr => Sr.City, O => O.MapFrom(S => S.Address.City))
                .ForMember(Sr => Sr.Country, O => O.MapFrom(S => S.Address.Country));
            
        }
    }
}
