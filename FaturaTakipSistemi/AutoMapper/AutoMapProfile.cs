﻿using AutoMapper;
using FaturaTakip.Data.Models;
using FaturaTakip.ViewModels;
using Microsoft.Build.Framework;

namespace FaturaTakip.AutoMapper
{
    public class AutoMapProfile : Profile
    {
        public AutoMapProfile()
        {
            CreateMap<Apartment, ApartmentVM>()
                .ForMember(dest => dest.LandlordName, opt => opt.MapFrom(src => src.Landlord.Name + " " + src.Landlord.LastName));

            CreateMap<RentedApartment, RentedApartmentVM>()
                .ForMember(dest => dest.ApartmentInfo, opt => opt.MapFrom
                (src => $"{nameof(src.Apartment.Block)} : {src.Apartment.Block} - {nameof(src.Apartment.Floor)} : {src.Apartment.Floor} - {nameof(src.Apartment.DoorNumber)} : {src.Apartment.DoorNumber}"))
                .ForMember(dest => dest.TenantName, opt => opt.MapFrom(src => src.Tenant.GovermentId + " - " + src.Tenant.Name + " " + src.Tenant.LastName));

            CreateMap<Landlord, LandlordSelectVM>()
                .ForMember(dest => dest.GovermentIdAndName, opt => opt.MapFrom(src => $"{src.GovermentId} - {src.Name} {src.LastName}"));

            CreateMap<Tenant, TenantSelectVM>()
                .ForMember(dest => dest.GovermentIdAndName, opt => opt.MapFrom(src => $"{src.GovermentId} - {src.Name} {src.LastName}"));

            CreateMap<Message, MessageVM>()
                .ForMember(dest => dest.LandlordFullName, opt => opt.MapFrom(src => $"{src.Landlord.Name} {src.Landlord.LastName}"))
                .ForMember(dest => dest.TenantFullName, opt => opt.MapFrom(src => $"{src.Tenant.Name} {src.Tenant.LastName}"))
                .ForMember(dest => dest.SenderName, opt => opt.MapFrom(src => $"{src.Sender.Name} {src.Sender.LastName}"));

        }
    }
}
//.ForMember(dest => dest.ApartmentInfo, opt => opt.MapFrom(src => "   " + src.Apartment.Block + "             " + src.Apartment.Floor + "             " + src.Apartment.DoorNumber))
