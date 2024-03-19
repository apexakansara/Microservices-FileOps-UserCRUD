namespace ConsumerService.Models;

public interface IMessageConsumer
{
    public Task ConsumeAsync();
}