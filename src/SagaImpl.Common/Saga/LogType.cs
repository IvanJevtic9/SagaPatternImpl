namespace SagaImpl.Common.Saga
{
    public enum LogType : byte
    {
        Start,
        End,
        Abort,
        Compesation
    }
}
