using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

public class RabbitMQBroker
{
    protected readonly ILogger<RabbitMQBroker> logger;
    protected readonly ConnectionFactory factory;
    protected readonly IConnection connection;
    protected readonly IModel channel;
    protected readonly string? mainQueueName;
    protected readonly IConfiguration config;
    protected Dictionary<string, object> mainQueueArgs;

    public RabbitMQBroker(IConfiguration config, ILogger<RabbitMQBroker> logger)
    {
        this.logger = logger;
        factory = new ConnectionFactory() { HostName = config["RabbitMQHostName"] };
        this.connection = factory.CreateConnection();
        this.channel = this.connection.CreateModel();
        this.mainQueueName = config["QueueNames:FileData"];
        this.config = config;
        this.DeclareDeadLetterQueue();
    }

    public void DeclareDeadLetterQueue()
    {
        var deadExchangeName = "dead-exchange";
        var deadLetterQueueName = this.config["QueueNames:DeadLetter"];

        this.channel.ExchangeDeclare(deadExchangeName, ExchangeType.Direct);
        this.channel.QueueDeclare(deadLetterQueueName,
            durable: true,
            exclusive: false,
            autoDelete: false);
        this.channel.QueueBind(deadLetterQueueName, 
            deadExchangeName, 
            deadLetterQueueName);

        this.mainQueueArgs = new Dictionary<string, object>{
            {"x-dead-letter-exchange", deadExchangeName},
            {"x-dead-letter-routing-key", deadLetterQueueName}
        };

        EventingBasicConsumer dlqConsumer = new EventingBasicConsumer(this.channel);
        dlqConsumer.Received += (model, eventArgs) =>
        {
            var body = eventArgs.Body.ToArray();
            string message = Encoding.UTF8.GetString(body);
            this.logger.LogError("Message from DLQ: {0}", message);
            // this.channel.BasicAck(eventArgs.DeliveryTag, false);
        };

        this.channel.BasicConsume(queue: deadLetterQueueName, 
            autoAck: false, 
            consumer: dlqConsumer);

    }
}