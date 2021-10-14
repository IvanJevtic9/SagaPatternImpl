using System.Collections.Generic;
using System.Linq;

namespace OrderService
{
    public class SagasOrchestration
    {
        private int sessionId = 0;
        private List<ISagaDefinition> DefinedSagas { get; set; }
        private Dictionary<int, SagaLog> SagaLogHistory { get; set; } // Saga session id / Logs


        public ISagaDefinition GetSaga(string name)
        {
            return DefinedSagas.FirstOrDefault(s => s.SagaName == name);
        }

        public void AddSaga(ISagaDefinition saga)
        {
            DefinedSagas.Add(saga);
        }

        public SagaLog GetSagaLog(int sessionId)
        {
            return SagaLogHistory[sessionId];
        }

        public int CreateNewSagaSession(ISagaDefinition saga)
        {
            sessionId++;

            SagaLogHistory.Add(sessionId, new SagaLog());

            saga.Start();

            return sessionId;
        }
    }
}
