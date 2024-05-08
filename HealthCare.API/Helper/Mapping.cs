using AutoMapper;
using HealthCare.Core.Entities;
using HealthCare.Core.Entities.Data;
using HealthCare.Core.Entities.identity;
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


            CreateMap<Patient, PatientWithHistoryAndObserverToReturnDto>();

            CreateMap<Doctor, DoctorToReturnDto>();

            CreateMap<AppUser, UserSearchToReturnDto>();

            CreateMap<Patient, PatientDataWithDoctorAndObserverToReturnDto>();

            CreateMap<Observer, ObserverToReturnDto>();

            CreateMap<History, HistoryToReturnDto>()
                .ForMember(HR => HR.HeartRate, O => O.MapFrom(H => H.UserData.HeartRate))
                .ForMember(HR => HR.Temperature, O => O.MapFrom(H => H.UserData.Temperature))
                .ForMember(HR => HR.ECG, O => O.MapFrom(H => H.UserData.ECG))
                .ForMember(HR => HR.Oxygen, O => O.MapFrom(H => H.UserData.Oxygen));

        }
    }
}
