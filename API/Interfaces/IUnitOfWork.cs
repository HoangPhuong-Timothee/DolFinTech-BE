namespace API.Interfaces
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        IStockRepository Stocks { get; }
        ICommentRepository Comments { get; }
        Task<int> CommitAsync();
    }
}