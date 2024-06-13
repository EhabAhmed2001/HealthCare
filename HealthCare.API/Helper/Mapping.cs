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


            CreateMap<Patient, PatientWithHistoryAndObserverToReturnDto>()
                .ForMember(Us => Us.PictureUrl, O => O.MapFrom<PictureResolver>());

            CreateMap<Doctor, DoctorToReturnDto>()
                .ForMember(Us => Us.PictureUrl, O => O.MapFrom<PictureResolver>());

            CreateMap<AppUser, UserSearchToReturnDto>()
                .ForMember(Us => Us.PictureUrl, O => O.MapFrom<PictureResolver>());

            CreateMap<Patient, PatientDataWithDoctorAndObserverToReturnDto>()
                .ForMember(Us => Us.PictureUrl, O => O.MapFrom<PictureResolver>()); 

            CreateMap<Observer, ObserverToReturnDto>()
                .ForMember(Us => Us.PictureUrl, O => O.MapFrom<PictureResolver>()); 

            CreateMap<History, HistoryToReturnDto>()
                .ForMember(HR => HR.HeartRate, O => O.MapFrom(H => H.UserData.HeartRate))
                .ForMember(HR => HR.Temperature, O => O.MapFrom(H => H.UserData.Temperature))
                .ForMember(HR => HR.ECG, O => O.MapFrom(H => H.UserData.ECG))
                .ForMember(HR => HR.Oxygen, O => O.MapFrom(H => H.UserData.Oxygen));

            CreateMap<HistoryDto, History>()
               .ForPath(H => H.UserData.HeartRate, O => O.MapFrom(HD => HD.HeartRate))
               .ForPath(H => H.UserData.Temperature, O => O.MapFrom(HD => HD.Temperature))
               .ForPath(H => H.UserData.ECG, O => O.MapFrom(HD => HD.ECG))
               .ForPath(H => H.UserData.Oxygen, O => O.MapFrom(HD => HD.Oxygen));

            CreateMap<AddressDto, Address>();

            CreateMap<AppUser, UserSearchToReturnDto>()
                .ForMember(Us => Us.PictureUrl, O => O.MapFrom<PictureResolver>());

        }
    }
}
