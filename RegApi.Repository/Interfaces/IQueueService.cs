namespace RegApi.Repository.Interfaces
{
    /// <summary>
    /// Defines methods for sending messages to a queue.
    /// </summary>
    public interface IQueueService
    {
        /// <summary>
        /// Sends a message of type T to the queue asynchronously.
        /// </summary>
        /// <typeparam name="T">The type of the message to be sent.</typeparam>
        /// <param name="message">The message to send.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task SendMessageAsync<T>(T message);
    }
}
