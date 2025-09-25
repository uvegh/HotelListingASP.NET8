using HotelListing.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace HotelListing.Controllers
{
    [ApiController]
    [Route("hotelApi/[controller]")]
    public class CountriesController : ControllerBase
    {
        private readonly HotelDBContext _context;
        public record CreateCountryDto(string Name, string ShortName);

        public CountriesController(HotelDBContext context)
        {
            _context = context;

        }

        [HttpGet]

        public async Task<ActionResult<IEnumerable<Country>>> GetCountries()
        {
            var countries = await _context.Countries.ToListAsync();
            if (countries != null) {
                return Ok(countries);
            }
            return NotFound();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Country>> GetCountry([FromRoute(Name = "id")] int Id)
        {
            var country = await _context.Countries.FindAsync(Id);
            if (country == null)
            {
                return NotFound();
            }
            return country;

        }



        [HttpPost]
        public async Task<ActionResult<Country>> PostCountry(Country country)
        {
            _context.Countries.Add(country);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCountry), new { id = country.Id }, country);


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

        public  async Task<ActionResult <Country >> UpdateCountry( int id, Country obj, CancellationToken ct)
        {
            if (id != obj.Id) return BadRequest("invalid Id");

            //change state to modified to prevent creating a new entity and just update the existing one
            //
            //var foundObj = await _context.Countries.FindAsync(id,ct);
            _context.Entry(obj).State = EntityState.Modified;
            try {
                await _context.SaveChangesAsync(ct);
                }
            catch (DBConcurrencyException)
            {
                if (!await _context.Countries.AnyAsync(res => res.Id == id))
                {

                    return NotFound();
                }
                else { throw; }
                
            }
            return Ok("Updated");
            
        }


        private bool ContExist(int id)
        {
            return (_context.Countries.Any(obj => obj.Id == id));

        }
    }

    
}