using API.Data;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly DolFinTechDbContext _context;
        public GenericRepository(DolFinTechDbContext context)
        {
            _context = context;
        }
        public async Task<List<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task CreateAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<T?> UpdateByIdAsync(int id, T entity)
        {
            var existedEntity = await _context.Set<T>().FindAsync(id);
            if(existedEntity == null)
            {   
                return null;
            }
            return existedEntity;
        }

        public async Task<T?> DeleteByIdAsync(int id)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            if(entity == null)
            {
                return null;    
            }
            _context.Set<T>().Remove(entity);
            return entity;
        }
    }
}