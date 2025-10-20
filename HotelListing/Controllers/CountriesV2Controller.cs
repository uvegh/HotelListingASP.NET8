
using AutoMapper;
using HotelListing.Contracts;
using HotelListing.Models.Country;
using HotelListing.Models.NewFolder;
using HotelListing.Models.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using HotelListing.Exceptions;
using Microsoft.AspNetCore.OData.Query;
using HotelListing.Core.Entities;
using HotelListing.Data;
using Microsoft.AspNetCore.OutputCaching;
namespace HotelListing.Controllers
{
    [ApiController]
    [Route("hotelApi/v{version:apiVersion}/countries")]
    [ApiVersion("2.0")]
    public class CountriesV2Controller : ControllerBase
    {
        //private readonly HotelDBContext _context;
        private readonly ILogger<CountriesController> _logger;
        private readonly IMapper _mapper;
        private readonly ICountryRepository _countryRepo;
        

        public CountriesV2Controller(HotelDBContext context,ILogger<CountriesController> logger, IMapper mapper, ICountryRepository countryRepository)
        {
            //_context = context;
            _logger = logger;
            _mapper = mapper;
            _countryRepo = countryRepository;

        }




        //hotelApi/v2/countries?StartIndex=5&PageNumber=1
       
        [HttpGet]
        [EnableQuery]
        [OutputCache(Duration =60,VaryByQueryKeys = new[] { "PageNumber","PageSize"})]
        public async Task<ActionResult<PageResult<GetCountryDto> >> GetCountries([FromQuery] QueryParameters queryParams)
        {

            var countries = await _countryRepo.GetAllPagedAsync<GetCountryDto>(queryParams);
            if(countries== null || countries.Items.Count ==0)
            {
                throw new NotFoundException (nameof(GetCountries),$"page {queryParams}"); 
            }
            return Ok(countries);
        }


        [EnableQuery]
        [HttpGet("filter")]
        public IQueryable<GetCountryDto> GetCountriesOData()
        {
            return _countryRepo.GetFilteredOData<GetCountryDto>();


                }



        [Authorize]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<CountryDto>> GetCountry([FromRoute(Name = "id")] int Id)
        {
            var country = await _countryRepo.GetDetails(Id); 
           
                if (country == null)
            {
                return NotFound();
            }
           var cont = _mapper.Map<CountryDto>(country);
            return Ok(cont);
           

        }

        [Authorize]

        [HttpPost]
        public async Task<ActionResult<Country>> PostCountry(CreateCountryDto CreateCountry)
        {
            
           

            var newObj = _mapper.Map<Country>(CreateCountry);
            Console.WriteLine(newObj);
            _logger.LogInformation("Creating a new country {@NewCountry}", newObj);


            await _countryRepo.CreateAsync(newObj);
            _logger.LogInformation("Creating a country {@newObj}", newObj.Id);
            return CreatedAtAction(nameof(GetCountry), new { id = newObj.Id }, newObj);


        }


        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Country>> DeleteCountry([FromRoute(Name = "id")] int Id)
        {
            var country = await _countryRepo.GetAsync(Id);

            if (country == null)
            {
                return NotFound();
            }
             await _countryRepo.DeleteAsync(Id);
            return Ok();
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
        public async Task<ActionResult<CountryDto>> UpdateCountry(int id, [FromBody] UpdateCountryDto UpdateCountryDto, CancellationToken ct)
        {
            //if (obj is not null) return BadRequest();
            if (id != UpdateCountryDto.Id) return BadRequest("invalid Id");

            //change state to modified to prevent creating a new entity and just update the existing one
            //
            //var foundObj = await _context.Countries.FindAsync(id,ct);
            //_context.Entry(obj).State = EntityState.Modified;
            //when you get the entity from the db context it is already being tracked so you dont need to set the state to modified
            //var UpdateCountryDto = await _context.Countries.FindAsync(id, ct);
            //var findObj = await _countryRepo.GetAsync(id);
            //if (findObj == null)
            //{
            //    throw new NotFoundException("Does not exist", id);
            //}
            try
            {
                var update = await _countryRepo.UpdateAsyncMap<UpdateCountryDto, Country>(id, UpdateCountryDto);
                if(update is null)
                {
                    throw new NotFoundException(nameof(UpdateCountry), UpdateCountryDto.Id);
                }
                return Ok(update);
            }
            catch (DBConcurrencyException)
            {

                return await _countryRepo.Exists(id) ? StatusCode(StatusCodes.Status409Conflict) : NotFound();
              

               
                
            }
        
            
            
        }


        private async Task<bool> ContExist(int id) => await _countryRepo.Exists(id);


    }

    
}