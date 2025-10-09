using AutoMapper;
using HotelListing.Contracts;
using HotelListing.Data;
using HotelListing.Models.Hotel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace HotelListing.Controllers
{

    [ApiController]
    [Route("hotelApi/[controller]")]
    public class HotelsController : ControllerBase
    {
        private readonly IHotelRepository _hotelRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<Hotel> _logger;
        public HotelsController(IHotelRepository hotelRepository, IMapper mapper, ILogger<Hotel> logger)
        {
            _hotelRepo = hotelRepository;
            _mapper = mapper;
            _logger = logger;
        }





        [HttpGet]

        public async Task<ActionResult<IEnumerable<HotelDto>>> GetHotels()
        {
            var hotels = await _hotelRepo.GetAllAsync();
            if (hotels == null) return NotFound();
            var res = _mapper.Map<List<HotelDto>>(hotels);

            return res;

        }

        [HttpGet("{id:int}")]

        public async Task<ActionResult<HotelDto>> GetHotel([FromRoute] int id)
        {
            var hotel = await _hotelRepo.GetDetails(id);
            if (hotel == null) return NotFound();
            var res = _mapper.Map<HotelDto>(hotel);
            return Ok(res);

        }
        [Authorize (Roles ="Administrator")]
        [HttpPost]

        public async Task<ActionResult<HotelDto>> CreateHotel([FromBody] CreateHotelDto obj)
        {
            var entity = _mapper.Map<Hotel>(obj);
            var hotel = await _hotelRepo.CreateAsync(entity);
            return Ok(hotel);

        }


        [HttpPut("{id:int}")]
        public async Task<ActionResult<HotelDto>> UpdateHotel ( [FromRoute] int id ,  [FromBody] EditHotelDto obj)
        {
            if (obj.Id != id) return BadRequest("Invalid Id");
         

            try
            {
                var hotel = await _hotelRepo.GetAsync(id);
                
                if (hotel == null) return NotFound();

                var mappedObj = _mapper.Map(obj, hotel);
                await _hotelRepo.UpdateAsync(mappedObj);
                return Ok(mappedObj);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await _hotelRepo.Exists(id))
                {
                  return  StatusCode(StatusCodes.Status409Conflict);
                }
                else
                {
                   return NotFound();
                }
                    
            }
            

        }


        [HttpDelete("{id:int}")]
        public async Task <ActionResult> DeleteHotel([FromRoute] int id) 
        {
            var entity = await _hotelRepo.Exists(id);
            if (entity)
            {
                await _hotelRepo.DeleteAsync(id);
                return StatusCode(StatusCodes.Status204NoContent);
            }
            return NotFound();



        }


    }

}