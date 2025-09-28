using AutoMapper;
using HotelListing.Data;
using HotelListing.Models.Country;

using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;
using System.Data;

namespace HotelListing.Controllers
{
    [ApiController]
    [Route("hotelApi/[controller]")]
    public class CountriesController : ControllerBase
    {
        private readonly HotelDBContext _context;
        private readonly ILogger<CountriesController> _logger;
        private readonly IMapper _mapper;
        public record CreateCountryDto(string Name, string ShortName);

        public CountriesController(HotelDBContext context,ILogger<CountriesController> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;


        }

        [HttpGet]

        public async Task<ActionResult<IEnumerable<GetCountryDto>>> GetCountries(CancellationToken ct)
        {
            var countries = await _context.Countries.ToListAsync(ct);
            
                var cont = _mapper.Map<List<GetCountryDto>>(countries);
                _logger.LogInformation("Get all countries {@cont}", cont);
            
            
            if (cont != null) {

                return Ok(cont);
            }
            return NotFound();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<CountryDto>> GetCountry([FromRoute(Name = "id")] int Id)
        {

            var country = await _context.Countries.Include(res => res.Hotels).FirstOrDefaultAsync(res => res.Id == Id);
                if (country == null)
            {
                return NotFound();
            }
           var cont = _mapper.Map<CountryDto>(country);
            return Ok(cont);
           

        }



        [HttpPost]
        public async Task<ActionResult<Country>> PostCountry(CreateCountryDto CreateCountry)
        {
            
            //create obj and map createCOuntry to country datatype

            var newObj = _mapper.Map<Country>(CreateCountry);
            Console.WriteLine(newObj);
            _logger.LogInformation("Creating a new country {@NewCountry}", newObj);
            

            _context.Countries.Add(newObj);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Creating a country {@newObj}", newObj.Id);
            return CreatedAtAction(nameof(GetCountry), new { id = newObj.Id }, newObj);


        }



        [HttpDelete("{id}")]
        public async Task<ActionResult<Country>> DeleteCountry([FromRoute(Name = "id")] int Id)
        {

            var country = await _context.Countries.FindAsync(Id);
            if (country != null)
            {
                _context.Countries.Remove(country);
                await _context.SaveChangesAsync();

                return Ok("Country deleted");
            }

            return NotFound();
        }






        //[HttpPut("{id}")]
        //public async Task<ActionResult <Country>> UpdateCountry([FromRoute(Name ="id")] int id,Country obj,CancellationToken? ct)
        //{
        //    if (id != obj.Id)
        //    {
        //        return BadRequest("Invalid id");
        //    }
        //    var foundCountry = await _context.Countries.FindAsync(id);

        //    if (foundCountry is null) return NotFound();
        //    foundCountry.ShortName = obj.ShortName;


        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch(DbUpdateConcurrencyException)
        //    {
        //        if (! await _context.Countries.AnyAsync(res=>res.Id==id))
        //        {
        //            Console.WriteLine("no id exist");
        //            throw;
        //        }

        //    }
        //    return NoContent();

        //}

        [HttpPut("{id}") ]
        public async Task<ActionResult<Country>> UpdateCountry(int id, [FromBody] UpdateCountryDto obj, CancellationToken ct)
        {
            if (id != obj.Id) return BadRequest("invalid Id");

            //change state to modified to prevent creating a new entity and just update the existing one
            //
            //var foundObj = await _context.Countries.FindAsync(id,ct);
            //_context.Entry(obj).State = EntityState.Modified;
            //when you get the entity from the db context it is already being tracked so you dont need to set the state to modified
            var UpdateCountryDto = await _context.Countries.FindAsync(id, ct);
            if (UpdateCountryDto is null) return NotFound("Does not exist");
            _mapper.Map(obj, UpdateCountryDto);
            try
            {
                await _context.SaveChangesAsync(ct);
                
                return Ok(UpdateCountryDto);
            }
            catch (DBConcurrencyException)
            {

              return  await ContExist(id, ct) ? StatusCode(StatusCodes.Status409Conflict) : NotFound();

               
                
            }
            
            
        }

        private Task<bool> ContExist(int id,CancellationToken? ct=null) => _context.Countries.AnyAsync(cont => cont.Id == id,ct?? CancellationToken.None);

        //private bool ContExist(int id)
        //{
        //    return (_context.Countries.Any(obj => obj.Id == id));

        //}


    }

    
}