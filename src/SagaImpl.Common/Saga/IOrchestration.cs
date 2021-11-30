using System.Threading.Tasks;

namespace SagaImpl.Common.Saga
{
    public interface IOrchestration
    {
        public Task StartAsync();
    }
}