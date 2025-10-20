using AutoMapper;
using AutoMapper.QueryableExtensions;
using HotelListing.Contracts;
using HotelListing.Data;
using HotelListing.Models.Country;
using HotelListing.Models.NewFolder;
using HotelListing.Models.Pagination;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {

        private readonly HotelDBContext _context;
        private readonly IMapper _mapper;

        public GenericRepository(HotelDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<T> CreateAsync(T entity)
        {
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;

        }

        public async Task<TResult> CreateMapAsync<TSource, TResult>(TSource Entity)
        {
            //map source entity to destination entity
            var res = _mapper.Map<T>(Entity);
            await _context.AddAsync(res);
            await _context.SaveChangesAsync();
            return _mapper.Map<TResult>(Entity);

        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetAsync(id);
            if (entity is not null)
                _context.Remove(entity);
            await _context.SaveChangesAsync();


        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();

        }
        public async Task<PageResult<TResult>> GetAllPagedAsync<TResult>(QueryParameters queryParameters)
        {
            //get total count of records in the table
            var totalSize = await _context.Set<T>().CountAsync();
            var items = await _context
                 //skip to index and take page size
                 .Set<T>().Skip(queryParameters.StartIndex).Take
//add mapper config so that it can be mapped to dto
(queryParameters.PageSize).ProjectTo<TResult>(_mapper.ConfigurationProvider).ToListAsync();

            return new PageResult<TResult>
            {
                Items = items,
                //total count of all items
                TotalCount = totalSize,
                //current number of items on pages
                RecordNumber = items.Count,

                //currwnt page
                CurrentPage = queryParameters.PageNumber,
            };
        }

        // For interactive clients / admin UIs
        //[EnableQuery]
        //[HttpGet("odata")]
        //public IQueryable<GetCountryDto> GetCountriesOData()
        //{
        //    return _context.Countries
        //        .AsNoTracking()
        //        .ProjectTo<GetCountryDto>(_mapper.ConfigurationProvider);
        //}



        public async Task<bool> Exists(int id)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            return entity is not null;

        }
        public async Task<T> GetAsync(int id)

        {
            var entity = await _context.Set<T>().FindAsync(id);
            //if(entity is null) throw new Exception($"Entity of type {typeof(T).Name} with id {id} not found"); 
            //if (entity is null) return null;
            return entity;



        }
        public async Task<T?> UpdateAsync(T Entity)
        {
            //if (Exists(id) is null) return null;
            //var obj=  await _context.Set<T>().FindAsync(id);
            //   if (obj is null) return null;
            _context.Set<T>().Update(Entity);
            await _context.SaveChangesAsync();
            return Entity;
        }

        //public async Task<T?> UpdateAsyncMap<TSource,TResult>(TSource entity,TResult res)
        //where TResult:class
        //    where TSource : class
        //{

        //    _mapper.Map(entity,res);

        //    _context.Set<T>().Update(res);
        //    await _context.SaveChangesAsync();


        //}


        public async Task<TResult?> UpdateAsyncMap<TSource, TResult>(int id, TSource source)
          where TResult : class
          where TSource : class
        {
            var entity = await _context.Set<T>().FindAsync(id);
            if (entity == null) return null;

         var result=   _mapper.Map(source, entity);
            _context.Update(entity);
            await _context.SaveChangesAsync();

            return _mapper.Map<TResult> (entity);
        }

       
    }
}