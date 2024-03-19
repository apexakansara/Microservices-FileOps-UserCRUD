using System.Text;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ConsumerService.Models;

public class RabbitMQConsumer : RabbitMQBroker, IMessageConsumer
{
    public RabbitMQConsumer(IConfiguration config, ILogger<RabbitMQConsumer> logger) : base(config, logger)
    {
    }

    public async Task ConsumeAsync()
    {
        // Default prefetch count = 1, and set autoAck = true
        this.channel.BasicQos(0, 1, true);
        this.channel.QueueDeclare(this.mainQueueName, false, false, false, this.mainQueueArgs);
        var consumer = new EventingBasicConsumer(this.channel);
        consumer.Received += async (model, eventArgs) => {
            var body = eventArgs.Body.ToArray();
            var meesage = Encoding.UTF8.GetString(body);
            if (!String.IsNullOrEmpty(meesage))
            {
                try
                {
                    Message obj = JsonConvert.DeserializeObject<Message>(meesage);
                    this.logger.LogInformation($"Message: {obj.Body}");
                    await HandleMessage(obj);

                    this.channel.BasicAck(eventArgs.DeliveryTag, true);
                }
                catch(Exception ex)
                {
                    this.logger.LogError($"Error processing message: {ex}");
                    this.channel.BasicReject(eventArgs.DeliveryTag, false);
                }
            }
        };
        this.channel.BasicConsume(this.mainQueueName, false, consumer);
        this.logger.LogInformation("Consume complete.");
        ConsumeDeadLetterQueue();
    }

    public void ConsumeDeadLetterQueue()
    {
        EventingBasicConsumer dlqConsumer = new EventingBasicConsumer(this.channel);
        dlqConsumer.Received += (model, eventArgs) =>
        {
            var body = eventArgs.Body.ToArray();
            string message = Encoding.UTF8.GetString(body);
            this.logger.LogError("Message from DLQ: {0}", message);
            this.channel.BasicAck(eventArgs.DeliveryTag, false);
        };

        this.channel.BasicConsume(queue: this.config["QueueNames:DeadLetter"], 
            autoAck: false, 
            consumer: dlqConsumer);
    }
    
    public async Task HandleMessage(Message msg)
    {
        this.logger.LogInformation($"Inside handle message with message: {msg.Body}");
        switch(msg.Type)
        {
            case "AddEmployee":
                {
                    UsersDbContext context = new UsersDbContext(this.config);
                    EmployeeRepository repository = new EmployeeRepository(context);
                    await repository.AddAsync(JsonConvert.DeserializeObject<Employee>(msg.Body));
                }
                break;

            default:
                break;
        }
    }
}