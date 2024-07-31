using API.Data;
using API.DTOs.Comment;
using API.Interfaces;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class CommentRepository : ICommentRepository //public class CommentRepository : GenericRepository<Comment>, ICommentRepository
    {
        private readonly DolFinTechDbContext _context;
        public CommentRepository(DolFinTechDbContext context) //public class CommentRepository(DolFinTechDbContext context) : base(context)
        {
            _context = context;
        }
         public async Task<List<Comment>> GetAllAsync()
        {
            return await _context.Comments.Include(c => c.AppUser).ToListAsync();
        }

        public async Task<Comment?> GetByIdAsync(int id)
        {
            return await _context.Comments.Include(c => c.AppUser).FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task CreateAsync(Comment comment)
        {
            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();
        }

          public async Task<Comment?> UpdateAsync(int id, UpdateCommentRequest request)
        {
            var existedComment = await _context.Comments.FirstOrDefaultAsync(c => c.Id == id);
            if(existedComment == null)
            {   
                return null;
            }
            existedComment.Title = request.Title;
            existedComment.Content = request.Content;
            await _context.SaveChangesAsync();
            return existedComment;
        }

        public async Task<Comment?> DeleteAsync(int id)
        {
            var comment = await _context.Comments.FirstOrDefaultAsync(c => c.Id == id);
            if(comment == null)
            {
                return null;    
            }
            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
            return comment;
        }
    }
}