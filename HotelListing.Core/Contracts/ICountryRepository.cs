using HotelListing.Data;
using HotelListing.Models.Country;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.Contracts
{
    public interface ICountryRepository : IGenericRepository<Country>
    {
        Task<Country>  GetDetails(int id);
         IQueryable<Tres> GetFilteredOData<Tres>();
        
    }
}
