namespace OrderService
{
    public class Program
    {
        static void Main(string[] args)
        {
            // Create SagaDefinition, Orchestration, SagaDbHistory  


        }
    }
}


/*
    Napraviti SagaFactory koja ce kreirati novu sagu (Rezultat Save-a ce sacuvati sagu u dbo.SagaDefinition)
    
    dbo.SagaDefinition - Saga definition.
    dbo.SagaExecutedHistory - Lista izvrsenih saga (sessionId , status, ... )
    
    OrchestrationService klasa 

    Odgovarajuci endpoint poziva odgovarajucu Sagu iz baze koja popunjava OrchestrationService klasu koja posle samo treba da se startuje i to je to ...

    Orchestration ide po listi ucesnika i oni se u logovima beleze ... Ako se stigne do kraja to je to ... 

    Success stanje - Done odmah belezimo u bazu.
    Abort stanje -  Poziva se rollBack (Ide se po logu i izvrsava se).

    Session details se belezi u SagaExecutedHistory.
*/