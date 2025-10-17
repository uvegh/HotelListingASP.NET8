using AutoMapper;
using AutoMapper.QueryableExtensions;
using HotelListing.Contracts;
using HotelListing.Core.Entities;
using HotelListing.Data;
using HotelListing.Models.Country;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.Repository
{
    public class CountryRepository:GenericRepository<Country>,ICountryRepository
    {
        private readonly HotelDBContext _context;
        private readonly IMapper _mapper;


        public CountryRepository(HotelDBContext context,IMapper mapper):base(context, mapper) {
            _context = context;
            _mapper = mapper;
        }
        //async Task<CountryDto> GetDetails(int id) => await _context.FindAsync(id);

        public async  Task<Country?> GetDetails(int id)
        {
            var country= await _context.Countries.Include(res => res.Hotels).FirstOrDefaultAsync(obj=>obj.Id==id);
            return country;
        }

        public IQueryable<Tres> GetFilteredOData<Tres>()
        {
            return _context.Countries.AsNoTracking().ProjectTo<Tres>(_mapper.ConfigurationProvider);
        }

        public IQueryable<Country> GetFilteredOData()
        {
            throw new NotImplementedException();
        }
    }
}
