using HotelListing.Contracts;
using HotelListing.Data;
using HotelListing.Models.Country;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.Repository
{
    public class CountryRepository:GenericRepository<Country>,ICountryRepository
    {
        private readonly HotelDBContext _context;


        public CountryRepository(HotelDBContext context):base(context) {
            _context = context;
        }
        //async Task<CountryDto> GetDetails(int id) => await _context.FindAsync(id);

        public async  Task<Country?> GetDetails(int id)
        {
            var country= await _context.Countries.Include(res => res.Hotels).FirstOrDefaultAsync(obj=>obj.Id==id);
            return country;
        }

        //Task ICountryRepository.GetDetails(int id)
        //{
        //    return GetDetails(id);
        //}
    }
}
