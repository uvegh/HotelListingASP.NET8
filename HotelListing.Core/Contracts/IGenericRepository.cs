using HotelListing.Models.NewFolder;
using HotelListing.Models.Pagination;

namespace HotelListing.Contracts
{
    public interface IGenericRepository<T> where T:class
    {
        Task<T> GetAsync(int id);
        Task<PageResult <TResult>> GetAllPagedAsync<TResult>(QueryParameters queryParameters);
        Task<List<T>> GetAllAsync();
        Task<T> CreateAsync(T entity);
        Task<T?> UpdateAsync( T entity);
        Task DeleteAsync(int id);
        Task<bool> Exists(int id);

    }

    
}
