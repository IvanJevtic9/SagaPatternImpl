using System;

namespace OrderService.Sagas
{
    // It's not best arhitecture (We can create some factory for Saga creation, and lot of other stuffs)
    public class CreateOrderSaga : ISagaDefinition
    {
        public string SagaName { get; } = "CreateOrder";

        public void Start()
        {
            throw new NotImplementedException();
        }
    }
}
