﻿using SagaImpl.Common.Abstraction.Interface;
using SagaImpl.Common.RabbitMQ;
using SagaImpl.Common.RabbitMQ.Abstraction;

namespace SagaImpl.InventoryService.Messaging.Receiver
{
    public class ReserveItemSubscriber : Subscriber<ReserveItemSubscriber>
    {
        public ReserveItemSubscriber(
            IConnectionProvider connectionProvider,
            ILoggerAdapter<ReserveItemSubscriber> logger,
            string exchangeName,
            string queueName,
            string exchangeType,
            int timeToLive = 30000)
        : base(connectionProvider, logger, queueName, exchangeName, exchangeType, timeToLive)
        { }
    }
}