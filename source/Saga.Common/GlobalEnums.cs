using System.ComponentModel;

namespace Saga.Common
{
    public enum OrderStatusEnum : byte
    {
        Pending,
        Approved,
        Rejected
    }

    public enum CurrencyEnum : byte
    {
        [Description("Euro")]
        EUR,
        [Description("Serbian dinar")]
        DIN,
        [Description("American Dollar")]
        USD,
        [Description("Britan pound")]
        GBP
    }

    public enum SagaStatusEnum : byte
    {
        Running = 1,
        Stopped = 2,
        Finished = 3
    }

    public enum LogType : byte
    {
        Start = 1,
        End = 2,
        Abort = 3,
        Compesation = 4
    }

    public enum EventType : byte
    {
        Success,
        Failure
    }
}
