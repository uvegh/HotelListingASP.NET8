using AutoMapper;
using HotelListing.Data;
using HotelListing.model.Country;
using HotelListing.Models.Country;
using HotelListing.Models.Hotel;

namespace HotelListing.Configurations
{
    public class MapperConfig:Profile
    {

        public MapperConfig()
        {
            CreateMap<CreateCountryDto, Country>().ReverseMap();
            CreateMap<EditCountryDto, Country>().ReverseMap();
            CreateMap<GetCountryDto, Country>().ReverseMap();
            CreateMap<CountryDto, Country>().ReverseMap();
            CreateMap<HotelDto, Hotel>().ReverseMap();
            CreateMap<BaseCountryDto, Country>().ReverseMap();
            CreateMap<BaseHotelDto, Hotel>().ReverseMap();
            CreateMap<HotelDto, Hotel>().ReverseMap();
            CreateMap<EditHotelDto, Hotel>().ReverseMap();
            CreateMap<CreateHotelDto, Hotel>().ReverseMap();
        }
    }
}
