namespace OrderService
{
    public interface ISagaDefinition
    {
        string SagaName { get; }

        //TODO - Razmisliti o strukturi -  Treba da bude tree. Svaki Node mora da ima Execute i Compesation method

        void Start();
    }
}