using SagaImpl.Common.Abstraction.Interface;
using System.Threading.Tasks;

namespace ELKOMERC.SharedKernel.Abstraction.Interface
{
    public interface IUnitOfWork : IDataConnection
    {
        IProcedureCall ProcedureCall { get; }

        Task SaveChangesAsync();
    }
}
