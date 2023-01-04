using Saga.Common;
using Saga.Common.Entities.Saga;
using Saga.Common.Saga.Abstraction;
using Saga.Order.Data;
using System.Text.Json;

namespace Saga.Order.Saga
{
    public class SagaOrchestration : IOrchestration
    {
        private readonly UnitOfWork _unitOfWork;

        public ISaga Saga { get; private set; }
        public SagaSession Session { get; private set; }
        public IDictionary<string, object> Context { get; private set; }

        public SagaOrchestration(UnitOfWork unitOfwork)
        {
            Context = new Dictionary<string, object>();

            _unitOfWork = unitOfwork;
        }

        public void SetSaga(ISaga saga)
        {
            Saga = saga;
        }

        public async Task AddContextData(string key, object data)
        {
            Context[key] = data;

            if(Session != null)
            {
                Session.Context = JsonSerializer.Serialize(Context);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        public async Task RemoveContextData(string key)
        {
            if (Context.ContainsKey(key))
            {
                Context.Remove(key);

                if (Session != null)
                {
                    Session.Context = JsonSerializer.Serialize(Context);
                    await _unitOfWork.SaveChangesAsync();
                }
            }
        }

        public async Task<bool> LoadFromCorrelationId(string correlationId)
        {
            try
            {
                Session = await _unitOfWork.SagaSession.GetFirstOrDefaultAsync(x => x.CorrelationId == correlationId, "Logs");
                Context = JsonSerializer.Deserialize<IDictionary<string, object>>(Session.Context);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public async Task Start()
        {
            Session = new()
            {
                SagaLevel = 1,
                CorrelationId = Guid.NewGuid().ToString(),
                SagaName = Saga.Name,
                Status = SagaStatusEnum.Running.ToString(),
                Context = JsonSerializer.Serialize(Context),
                Logs = new List<SagaLog>()
            };

            await _unitOfWork.SagaSession.AddAsync(Session);
            await _unitOfWork.SaveChangesAsync();

            var steps = Saga.Steps.Where(x => x.Level == Session.SagaLevel).ToList();

            foreach (var step in steps)
            {
                await step.Step.SetContext(Session.Id, Session.CorrelationId, Context);
                await step.Step.Commit();
            }
        }

        public async Task Complete()
        {
            if (Session.CorrelationId == null)
            {
                throw new ArgumentNullException("Context not provided.");
            }

            var result = Saga.Steps.Any(x => x.Level == Session.SagaLevel);

            if (result)
            {
                return;
            }

            foreach (var step in Saga.Steps)
            {
                await step.Step.SetContext(Session.Id, Session.CorrelationId, Context);
                await step.Step.Complete();
            }

            await SaveSagaStatus(SagaStatusEnum.Finished);
        }

        public async Task Continue()
        {
            if (Session.CorrelationId == null)
            {
                throw new ArgumentNullException("Context not provided.");
            }

            if (Session.RollbackTrigger)
            {
                return;
            }

            var steps = Saga.Steps.Where(x => x.Level == Session.SagaLevel).ToList();
            foreach (var step in steps)
            {
                if (!Session.Logs.Any(x => 
                    x.Step == step.Step.StepName &&
                    (x.TypeId == LogType.End || x.TypeId == LogType.Abort))
                )
                {
                    // Still the whole level is not completed , do not continue saga
                    return;
                }
            }

            Session.SagaLevel++;
            steps = Saga.Steps.Where(x => x.Level == Session.SagaLevel).ToList();

            if (steps.Count() == 0)
            {
                await Complete();
                return;
            }

            foreach (var step in steps)
            {
                await step.Step.SetContext(Session.Id, Session.CorrelationId, Context);
                await step.Step.Commit();
            }

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task Rollback()
        {
            if (Session.CorrelationId == null)
            {
                throw new ArgumentNullException("Context not provided.");
            }

            Session.RollbackTrigger = true;
            await _unitOfWork.SaveChangesAsync();

            var logs = Session.Logs.Where(x => x.TypeId == LogType.End).ToList();

            foreach (var log in logs)
            {
                var step = Saga.Steps.FirstOrDefault(x => x.Step.StepName == log.Step).Step;
                await step.SetContext(Session.Id, Session.CorrelationId, Context);
                await step.RollBack();
            }
        }

        private async Task SaveSagaStatus(SagaStatusEnum status)
        {
            Session.Status = status.ToString();
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
