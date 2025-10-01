using HotelListing.Contracts;
using HotelListing.Data;
using HotelListing.Models.Hotel;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.Repository
{
    public class HotelRepository:GenericRepository<Hotel>,IHotelRepository
    {

        private readonly HotelDBContext _context;
        private readonly ILogger<HotelRepository> _logger;
       public HotelRepository(HotelDBContext context, ILogger<HotelRepository> logger):base(context)
        {
            _context = context;
            _logger = logger;
            
        }

        public async    Task<Hotel?> GetDetails(int id)
        {
            var hotel = await _context.Hotels.Include(q => q.Country).FirstOrDefaultAsync(res => res.Id==id);
            
            return hotel;


        }

       
    }
}
