using RabbitMQ.Client;

namespace HelloRabbitMq.Watermark.Services
{
    public class RabbitMqClientService : IDisposable
    {
        private readonly ConnectionFactory _connectionFactory;
        private IConnection _connection;
        private IModel _channel;
        private readonly ILogger<RabbitMqClientService> _logger;

        public static string ExchangeName = "ImageExchange";
        public static string RoutingWatermark = "watermark-route";
        public static string QueueName = "queue-watermark-image";

        public RabbitMqClientService(ConnectionFactory connectionFactory, ILogger<RabbitMqClientService> logger)
        {
            _connectionFactory = new ConnectionFactory();
            _logger = logger;
        }

        public IModel Connect()
        {
            _connection = _connectionFactory.CreateConnection();

            if (_channel is { IsOpen: true })
            {
                return _channel;
            }

            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(ExchangeName, type: "direct", durable: true, autoDelete: false);
            _channel.QueueDeclare(QueueName, durable: true, exclusive: false, autoDelete: false, null);
            _channel.QueueBind(exchange: ExchangeName, queue: QueueName, routingKey: RoutingWatermark);
            _logger.LogInformation("Connected to RabbitMq");

            return _channel;
        }

        public void Dispose()
        {
            _channel?.Close();
            _channel?.Dispose();
            //_channel = default;
            _connection?.Close();
            _connection?.Dispose();

            _logger.LogInformation("Connection is lost with RabbitMq");
        }
    }
}
