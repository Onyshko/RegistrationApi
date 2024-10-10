using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using RegApi.EmailSender.Models;
using RegApi.EmailSender.Services;
using RegApi.Shared.Models;

namespace EmailSender
{
    /// <summary>
    /// Represents a background service that listens for messages from Azure Service Bus and sends emails based on the received messages.
    /// </summary>
    public class Worker : BackgroundService
    {
        /// <summary>
        /// Provides access to the service provider for dependency injection.
        /// </summary>
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="Worker"/> class with the specified service provider.
        /// </summary>
        /// <param name="serviceProvider">The service provider used for resolving dependencies.</param>
        public Worker(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Executes the background service, continuously receiving messages from the Service Bus and sending emails.
        /// </summary>
        /// <param name="stoppingToken">Token to signal when the service should stop.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                // Retrieves the required services from the service provider scope
                var client = scope.ServiceProvider.GetRequiredService<ServiceBusClient>();
                var receiver = scope.ServiceProvider.GetRequiredService<ServiceBusReceiver>();
                var emailSenderService = scope.ServiceProvider.GetRequiredService<IEmailSenderService>();

                try
                {
                    // Continuously listens for messages from the Service Bus
                    while (true)
                    {
                        var receivedMessage = await receiver.ReceiveMessageAsync();

                        if (receivedMessage != null)
                        {
                            // Deserializes the message body and sends the email
                            string messageBodyJson = receivedMessage.Body.ToString();
                            var messageBody = JsonConvert.DeserializeObject<EmailSenderModel>(messageBodyJson!);

                            await emailSenderService.SendEmailAsync(new Message(messageBody!));

                            // Marks the message as complete after successful processing
                            await receiver.CompleteMessageAsync(receivedMessage);
                        }
                        else
                        {
                            Console.WriteLine("No more messages to receive.");
                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Logs any exceptions that occur during message processing
                    Console.WriteLine($"Exception: {ex.Message}");
                }
                finally
                {
                    // Closes the receiver and disposes of the Service Bus client
                    await receiver.CloseAsync();
                    await client.DisposeAsync();
                }
            }
        }
    }
}
