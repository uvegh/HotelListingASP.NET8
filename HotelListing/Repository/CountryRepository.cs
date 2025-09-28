using HotelListing.Contracts;
using HotelListing.Data;

namespace HotelListing.Repository
{
    public class CountryRepository:GenericRepository<Country>,ICountryRepository
    {
        private readonly HotelDBContext _context;


        public CountryRepository(HotelDBContext context):base(context) {
            _context = context;
        }
    }
}
