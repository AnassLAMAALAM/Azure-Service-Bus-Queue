using Microsoft.Azure.ServiceBus;
using System.Text;
using System.Text.Json;
namespace Sender.Services
{
    public class ServiceBusService : IServiceBusService
    {
        public IConfiguration config { get; set; }

        public ServiceBusService(IConfiguration config) 
        {
            this.config = config;
        }

        public async Task SendMessageAsync<T>(T ServiceBusMessage, string QueueName)
        {
            var QueueClient = new QueueClient(this.config.GetConnectionString("AzureServiceBus"), QueueName);
            string MessageBody = JsonSerializer.Serialize(ServiceBusMessage);
            var Message = new Message(Encoding.UTF8.GetBytes(MessageBody));
            await QueueClient.SendAsync(Message);
        }

    }
}
