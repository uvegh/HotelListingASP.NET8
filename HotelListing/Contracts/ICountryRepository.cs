using HotelListing.Data;
using HotelListing.Models.Country;

namespace HotelListing.Contracts
{
    public interface ICountryRepository : IGenericRepository<Country>
    {
        Task<Country>  GetDetails(int id);
    }
}
