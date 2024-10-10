using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using RegApi.Repository.Interfaces;

namespace RegApi.Repository.Implementations
{
    /// <summary>
    /// Provides methods for sending messages to an Azure Service Bus queue.
    /// </summary>
    public class QueueService : IQueueService
    {
        /// <summary>
        /// The service bus sender used to send messages to the queue.
        /// </summary>
        private readonly ServiceBusSender _serviceBusSender;

        /// <summary>
        /// Initializes a new instance of the <see cref="QueueService"/> class with the specified service bus sender.
        /// </summary>
        /// <param name="queueClient">The service bus sender for sending messages.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="queueClient"/> is null.</exception>
        public QueueService(ServiceBusSender queueClient)
        {
            _serviceBusSender = queueClient ?? throw new ArgumentNullException(nameof(queueClient));
        }

        /// <summary>
        /// Sends a message to the Azure Service Bus queue asynchronously.
        /// </summary>
        /// <typeparam name="T">The type of the message to send.</typeparam>
        /// <param name="message">The message to be sent.</param>
        /// <returns>A task representing the asynchronous send operation.</returns>
        public async Task SendMessageAsync<T>(T message)
        {
            string messageBody = JsonConvert.SerializeObject(message);
            var messageByte = new ServiceBusMessage(messageBody);

            await _serviceBusSender.SendMessageAsync(messageByte);
        }
    }
}
