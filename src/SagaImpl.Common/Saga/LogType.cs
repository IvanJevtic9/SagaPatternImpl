using SagaImpl.Common.Apstraction.Interface;

namespace SagaImpl.Common.Saga
{
    public class LogType : IEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
