namespace API.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        Task CreateAsync(T entity);
        Task<T?> UpdateByIdAsync(int id, T entity);
        Task<T?> DeleteByIdAsync(int id); 
    }
}