namespace Saga.Common.Data.Abstraction
{
    public interface IUnitOfWork : IDisposable
    {
        Task SaveChangesAsync();
    }
}
