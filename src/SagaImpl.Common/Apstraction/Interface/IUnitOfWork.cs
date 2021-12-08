using System.Threading.Tasks;

namespace SagaImpl.Common.Abstraction.Interface
{
    public interface IUnitOfWork
    {
        Task SaveChangesAsync();
    }
}
