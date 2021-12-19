using System.Threading.Tasks;

namespace SagaImpl.Common.Saga.Abstaction
{
    public interface IOrchestration
    {
        Task StartAsync(object input);
    }
}