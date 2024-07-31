using API.Data;
using API.Interfaces;
using API.Repositories;

namespace API.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DolFinTechDbContext _context;
        public IStockRepository Stocks { get; private set; }
        public ICommentRepository Comments { get; private set; }
        public UnitOfWork(DolFinTechDbContext context)
        {
            _context = context;
            Stocks = new StockRepository(_context);
            Comments = new CommentRepository(_context);
        }

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async ValueTask DisposeAsync()
        {
            await _context.DisposeAsync();
        }
    }
}