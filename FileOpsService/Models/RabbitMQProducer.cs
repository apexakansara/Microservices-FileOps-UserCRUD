using System.Text;
using RabbitMQ.Client;

namespace FileOpsService.Models;

public class RabbitMQProducer : RabbitMQBroker, IMessageProducer
{
    public RabbitMQProducer(IConfiguration config, ILogger<RabbitMQBroker> logger) : base(config, logger)
    {
    }

    public void Publish(List<string> messages)
    {
        using (var connection = this.factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            channel.QueueDeclare(queue: this.mainQueueName, 
                durable: false, 
                exclusive: false, 
                autoDelete: false,
                this.mainQueueArgs);

            foreach (string message in messages)
            {
                byte[] data = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(exchange: String.Empty, 
                                    routingKey: this.mainQueueName, 
                                    basicProperties: null, 
                                    data);
                this.logger.LogInformation($"{message} published.");
            }
        }
    }
}