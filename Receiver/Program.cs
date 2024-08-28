using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Receiver;
class Program
{
    const string ConnectionString = "";
    const string QueueName = "";
    static IQueueClient? QueueClient;
    static async Task Main(string[] args)
    {
        QueueClient = new QueueClient(ConnectionString, QueueName);

        var MessageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler) { AutoComplete = false, MaxConcurrentCalls = 1 };

        QueueClient.RegisterMessageHandler(ProcessMessagesAsync,MessageHandlerOptions);

        Console.ReadLine();

        await QueueClient.CloseAsync();
    }

    private static async Task ProcessMessagesAsync(Message message, CancellationToken token)
    {
        var JsonString = Encoding.UTF8.GetString(message.Body);
        var Message = JsonSerializer.Deserialize<string>(JsonString);
        Console.WriteLine($"Message Received : {Message}");
        await QueueClient.CloseAsync();
    }

    private static Task ExceptionReceivedHandler(ExceptionReceivedEventArgs args)
    {
        Console.Write($"Message handler exception : { args.Exception }");
        return Task.CompletedTask;
    }

}
