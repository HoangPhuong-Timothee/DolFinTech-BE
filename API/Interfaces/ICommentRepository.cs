using API.DTOs.Comment;
using API.Models;

namespace API.Interfaces
{
    public interface ICommentRepository
    // : IGenericRepository<Comment>
    {
        Task<List<Comment>> GetAllAsync();
        Task<Comment?> GetByIdAsync(int id);
        Task CreateAsync(Comment comment);
        Task<Comment?> UpdateAsync(int id, UpdateCommentRequest request);
        Task<Comment?> DeleteAsync(int id);
    }
}