namespace FileOpsService.Models;

public interface IMessageProducer
{
    public void Publish(List<string> messages){}
}