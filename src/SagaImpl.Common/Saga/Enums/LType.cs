namespace SagaImpl.Common.Saga.Enums
{
    public enum SagaStatus : byte
    {
        Running = 1,
        Stopped = 2,
        Finished = 3
    }

    public enum LType : byte
    {
        Start = 1,
        End = 2,
        Abort = 3,
        Compesation = 4
    }
}
