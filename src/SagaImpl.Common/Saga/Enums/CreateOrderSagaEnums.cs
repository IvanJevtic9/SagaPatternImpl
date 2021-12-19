namespace SagaImpl.Common.Saga.Enums
{
    public enum CreateOrderSagaCommand : byte
    {
        CreateOrder,
        RejectOrder,
        ReserveItems,
        UnreserveItems,
        PayOrder,
        RefundMoney,
        FinishSaga
    }

    public enum CreateOrderSagaEvents : byte
    {
        StartSaga,
        CreateOrder,
        ReserveItems,
        PayOrder
    }
}
