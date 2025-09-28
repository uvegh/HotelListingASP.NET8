using HotelListing.Contracts;
using HotelListing.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class {

        private readonly HotelDBContext _context;

        public GenericRepository( HotelDBContext context) {
            _context = context;
        }
        public async Task<T> CreateAsync(T entity) {
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
         
        }

        public async Task DeleteAsync(int id) {
            var entity = await GetAsync(id);
            if (entity is not  null) 
            _context.Remove(entity);
            await _context.SaveChangesAsync();
            

        }

        public async Task <List<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();

        }
         
        public async Task <bool> Exists(int id)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            return entity is not null;                                          

        }
        public async Task <T> GetAsync(int id)

        {
            var entity = await _context.Set<T>().FindAsync(id);
            if(entity is null) throw new Exception($"Entity of type {typeof(T).Name} with id {id} not found"); 
            return entity;

           

        }
        public async Task <T?> UpdateAsync(int id, T Entity)
        {
            if (Exists(id) is null) return null;
         var obj=  await _context.Set<T>().FindAsync(id);
            if (obj is null) return null;
            _context.Set<T>().Update(Entity);
            await _context.SaveChangesAsync();
            return Entity;
        }




    }
}
