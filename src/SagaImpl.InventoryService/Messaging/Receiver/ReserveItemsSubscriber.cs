using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SagaImpl.Common;
using SagaImpl.Common.Abstraction.Interface;
using SagaImpl.Common.RabbitMQ.Abstraction;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SagaImpl.InventoryService.Messaging.Receiver
{
    public class ReserveItemsSubscriber : BackgroundService
    {
        private readonly ILoggerAdapter<ReserveItemsSubscriber> logger;
        private readonly IModel channel;
        private readonly string queueName;

        public ReserveItemsSubscriber(
            IConnectionProvider connectionProvider,
            ILoggerAdapter<ReserveItemsSubscriber> logger,
            string queueName,
            string exchangeType,
            int timeToLive = 30000)
        {
            this.logger = logger;
            this.queueName = queueName;

            channel = connectionProvider.GetConnection().CreateModel();

            var ttl = new Dictionary<string, object>
            {
                {"x-message-ttl", timeToLive }
            };

            channel.ExchangeDeclare(CommonConstants.ORDER_SERVICE_EXCHANGE, exchangeType, arguments: ttl);
            channel.QueueDeclare(queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);
            channel.QueueBind(queueName, CommonConstants.ORDER_SERVICE_EXCHANGE, queueName);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (sender, e) =>
            {
                var body = e.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                logger.LogInformation($"Exchange: {CommonConstants.ORDER_SERVICE_EXCHANGE} received rbmq message. Routing key: {queueName}");

                channel.BasicAck(e.DeliveryTag, false);
            };

            channel.BasicConsume(queueName, false, consumer);

            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            channel.Close();
            base.Dispose();
        }
    }
}


//namespace OrderApi.Messaging.Receive.Receiver.v1
//{
//    public class CustomerFullNameUpdateReceiver : BackgroundService
//    {
//        private IModel _channel;
//        private IConnection _connection;
//        private readonly ICustomerNameUpdateService _customerNameUpdateService;
//        private readonly string _hostname;
//        private readonly string _queueName;
//        private readonly string _username;
//        private readonly string _password;

//        public CustomerFullNameUpdateReceiver(ICustomerNameUpdateService customerNameUpdateService, IOptions<RabbitMqConfiguration> rabbitMqOptions)
//        {
//            _hostname = rabbitMqOptions.Value.Hostname;
//            _queueName = rabbitMqOptions.Value.QueueName;
//            _username = rabbitMqOptions.Value.UserName;
//            _password = rabbitMqOptions.Value.Password;
//            _customerNameUpdateService = customerNameUpdateService;
//            InitializeRabbitMqListener();
//        }

//        private void InitializeRabbitMqListener()
//        {
//            var factory = new ConnectionFactory
//            {
//                HostName = _hostname,
//                UserName = _username,
//                Password = _password
//            };

//            _connection = factory.CreateConnection();
//            _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
//            _channel = _connection.CreateModel();
//            _channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
//        }

//        protected override Task ExecuteAsync(CancellationToken stoppingToken)
//        {
//            stoppingToken.ThrowIfCancellationRequested();

//            var consumer = new EventingBasicConsumer(_channel);
//            consumer.Received += (ch, ea) =>
//            {
//                var content = Encoding.UTF8.GetString(ea.Body.ToArray());
//                var updateCustomerFullNameModel = JsonConvert.DeserializeObject<UpdateCustomerFullNameModel>(content);

//                HandleMessage(updateCustomerFullNameModel);

//                _channel.BasicAck(ea.DeliveryTag, false);
//            };
//            consumer.Shutdown += OnConsumerShutdown;
//            consumer.Registered += OnConsumerRegistered;
//            consumer.Unregistered += OnConsumerUnregistered;
//            consumer.ConsumerCancelled += OnConsumerCancelled;

//            _channel.BasicConsume(_queueName, false, consumer);

//            return Task.CompletedTask;
//        }

//        private void HandleMessage(UpdateCustomerFullNameModel updateCustomerFullNameModel)
//        {
//            _customerNameUpdateService.UpdateCustomerNameInOrders(updateCustomerFullNameModel);
//        }

//        private void OnConsumerCancelled(object sender, ConsumerEventArgs e)
//        {
//        }

//        private void OnConsumerUnregistered(object sender, ConsumerEventArgs e)
//        {
//        }

//        private void OnConsumerRegistered(object sender, ConsumerEventArgs e)
//        {
//        }

//        private void OnConsumerShutdown(object sender, ShutdownEventArgs e)
//        {
//        }

//        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
//        {
//        }

//        public override void Dispose()
//        {
//            _channel.Close();
//            _connection.Close();
//            base.Dispose();
//        }
//    }
//}
