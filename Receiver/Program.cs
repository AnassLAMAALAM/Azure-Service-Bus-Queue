using Microsoft.Azure.ServiceBus;
using System.Text;
using System.Text.Json;

namespace Receiver
{
    class Program
    {
        const string ConnectionString = "Endpoint=sb://service-bus-anass.servicebus.windows.net/;SharedAccessKeyName=SharedQueuePolicyManagement;SharedAccessKey=CxAfTyz4FxZfZ1UaAQeqMRuETTQR/b2p3+ASbN97ZMc=";
        const string QueueName = "anass-queue";
        static IQueueClient? QueueClient;

        static async Task Main(string[] args)
        {
            // Initialize the QueueClient
            QueueClient = new QueueClient(ConnectionString, QueueName);

            // Configure the message handler options
            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                AutoComplete = false, // Manually complete the message after processing
                MaxConcurrentCalls = 1 // Ensure messages are processed one at a time
            };

            // Register the message handler
            QueueClient.RegisterMessageHandler(ProcessMessagesAsync, messageHandlerOptions);

            // Keep the console open
            Console.WriteLine("Listening for messages...");
            Console.ReadLine();

            // Close the client after the console is closed
            await QueueClient.CloseAsync();
        }

        private static async Task ProcessMessagesAsync(Message message, CancellationToken token)
        {
            // Deserialize and display the message
            var jsonString = Encoding.UTF8.GetString(message.Body);
            var receivedMessage = JsonSerializer.Deserialize<string>(jsonString);
            Console.WriteLine($"Message Received: {receivedMessage}");

            // Complete the message so it is removed from the queue
            await QueueClient.CompleteAsync(message.SystemProperties.LockToken);
        }

        private static Task ExceptionReceivedHandler(ExceptionReceivedEventArgs args)
        {
            // Handle any errors that occur during message processing
            Console.WriteLine($"Message handler exception: {args.Exception.Message}");
            return Task.CompletedTask;
        }
    }
}
