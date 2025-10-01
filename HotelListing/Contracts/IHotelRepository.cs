using HotelListing.Data;

namespace HotelListing.Contracts
{
    public interface IHotelRepository:IGenericRepository<Hotel>

    {
        Task<Hotel?> GetDetails(int id);

    }
}
