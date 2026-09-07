
namespace Prtcl.Rabbitmq
{
    public interface IRabbitmqService
    {
        Task<bool> PublishAsync<T>(T data, string queueName);
    }
}
