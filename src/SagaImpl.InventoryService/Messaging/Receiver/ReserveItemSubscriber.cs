using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SagaImpl.Common;
using SagaImpl.Common.Abstraction.Interface;
using SagaImpl.Common.RabbitMQ.Abstraction;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SagaImpl.InventoryService.Messaging.Receiver
{
    public class ReserveItemSubscriber : ISubscriber
    {
        private readonly ILoggerAdapter<ReserveItemSubscriber> logger;
        private readonly IModel channel;
        private readonly string queueName;

        private bool disposed;

        public ReserveItemSubscriber(
            IConnectionProvider connectionProvider,
            ILoggerAdapter<ReserveItemSubscriber> logger,
            string queueName,
            string exchangeType,
            int timeToLive = 30000)
        {
            this.logger = logger;
            this.queueName = queueName;

            var ttl = new Dictionary<string, object>
            {
                {"x-message-ttl", timeToLive }
            };

            channel = connectionProvider.GetConnection().CreateModel();
            channel.ExchangeDeclare(CommonConstants.ORDER_SERVICE_EXCHANGE, exchangeType, arguments: ttl);
            channel.QueueDeclare(queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);
            channel.QueueBind(queueName, CommonConstants.ORDER_SERVICE_EXCHANGE, queueName);
        }

        public void SubscribeAsync(Func<string, IDictionary<string, object>, Task<bool>> callback)
        {
            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += async (sender, e) =>
            {
                var body = e.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                bool success = await callback.Invoke(message, e.BasicProperties.Headers);

                logger.LogInformation($"Exchange: {CommonConstants.ORDER_SERVICE_EXCHANGE} received rbmq message. Routing key: {queueName}");

                if(success) channel.BasicAck(e.DeliveryTag, false);
            };

            channel.BasicConsume(queueName, false, consumer);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed) return;
            if (disposing) channel?.Close();
            disposed = true;
        }
    }
}