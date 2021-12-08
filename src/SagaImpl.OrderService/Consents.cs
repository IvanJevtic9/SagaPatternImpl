namespace SagaImpl.OrderService
{
    public static class Consents
    {
        public static class SchemaNames
        {
            public const string ORDER_SCHEMA = "OrderService";
            public const string SAGA_SCHEMA = "Saga";
        }

        public static class SagaNames
        {
            public const string CREATE_ORDER = "CreateOrder";
        }

        public static class OrchestrationNames
        {
            public const string CREATE_ORDER = "CreateOrder";
        }
    }
}
