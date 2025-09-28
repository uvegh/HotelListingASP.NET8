namespace HotelListing.Contracts
{
    public interface IGenericRepository<T> where T:class
    {
        Task<T> GetAsync(int id);
        Task<List<T>> GetAllAsync();
        Task<T> CreateAsync(T entity);
        Task<T?> UpdateAsync(int id, T entity);
        Task DeleteAsync(int id);
        Task<bool> Exists(int id);

    }

    
}
