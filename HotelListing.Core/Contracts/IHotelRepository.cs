using HotelListing.Core.Entities;
using HotelListing.Data;

namespace HotelListing.Contracts
{
    public interface IHotelRepository:IGenericRepository<Hotel>

    {
        Task<Hotel?> GetDetails(int id);

    }
}
