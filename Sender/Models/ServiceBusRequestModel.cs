namespace Sender.Models
{
    public class ServiceBusRequestModel
    {
        public string QueueName { get; set; }
        public string Message { get; set; }
    }
}
