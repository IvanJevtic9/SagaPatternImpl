using System.Threading.Tasks;

namespace ELKOMERC.SharedKernel.Abstraction.Interface
{
    public interface IUnitOfWork
    {
        Task SaveChangesAsync();
    }
}
