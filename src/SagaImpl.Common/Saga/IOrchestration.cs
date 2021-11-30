using System.Threading.Tasks;

namespace SagaImpl.Common.Saga
{
    public interface IOrchestration
    {
        public Task StartAsync();
    }
}
Saga orchestration
	
	CreateOrderSaga:
	
	Order->Inventory
			< -
			->Payment
			< -


	Orchestrator | 1 / 2 / 3 |

	----------------------------------------------------------------

	SagaDefinition - SagaId | SagaName | LastPhase


	SagaSteps - SagaId | Phase | Method | RollBackMethod -

	SagaSession - SessionId | SagaDefID | TimeCreated | Status | ... ReqInformation
	
	SagaHistory		-   SessionId | SagaDefId |  SagaSteps | LogType | Log | TimeCreated | 
	
	Problem kako konstruisati dinamicki input za naredni cvor.
	
	----------------------------------------------------------------
	
	SagaDefinition 
	
	1 CreateOrderSaga 3
	
	SagaSteps
	
 	id  sg	ph
	1  	1	1	 CreateOrder  DeleteOrder
	2  	1  	2  	 ReserveItems DeleteReservation
	3  	1  	3  	 PayOrder     ReturnFunds
	
	SagaHistory
-------------------------------	
	1,
	1 - CreateOrder,
	Saga.Start,
	{
		userId: 12151,
		currency: eur
	},
	"2021-11-30 15:38:11"
------------------------------ -
-------------------------------
	1, -session Id
	1, -CreateOrder,
	Saga.End,
	{
id: 12
		userId: 12151,
		CreatedDate: "2021-11-30",
		Status: "Pending"
		totalPrice: 0
		currency: eur
	},
	"2021-11-30 15:38:11"
------------------------------ -
-------------------------------
	1,
	2 - ReserveItems,
	Saga.Start,
	{
	[
			{
	id: 12
				number: 33
			},
			{
	id: 13
				number: 44
			},
		]
	},
	"2021-11-30 15:38:11"
------------------------------ -
-------------------------------
	1,
	2 - ReserveItems,
	Saga.End,
	{
	[

		... -Created Items
		]
	},
	"2021-11-30 15:38:11"
------------------------------ -
-------------------------------
	1,
	3 - PayOrder,
	Saga.Start,
	{
userId: 12151,
		price: 214124,
		currency: eur
	},
	"2021-11-30 15:38:11"
------------------------------ -
-------------------------------
	1,
	3 - PayOrder,
	Saga.Abort,
	{
message: Not enaugh funds
	},
	"2021-11-30 15:38:11"
------------------------------ -
-------------------------------
	1,
	2 - DeleteReservation,
	Saga.Compesation,
	{
	...
	},
	"2021-11-30 15:38:11"
------------------------------ -
-------------------------------
	1,
	2 - DeleteOrder,
	Saga.Compesation,
	{
	...
	},
	"2021-11-30 15:38:11"
------------------------------ -

