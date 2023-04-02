using AutoMapper;
using FaturaTakip.Data.Models;
using FaturaTakip.ViewModels;

namespace FaturaTakip.AutoMapper
{
    public class AutoMapProfile : Profile
    {
        public AutoMapProfile()
        {
            CreateMap<Apartment, ApartmentVM>()
                .ForMember(dest => dest.LandlordName, opt => opt.MapFrom(src => src.Landlord.Name + " " + src.Landlord.LastName));
        } 
    }
}
