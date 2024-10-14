using Azure.Messaging.ServiceBus;
using EmailSender.Models;
using EmailSender.Services;
using Microsoft.Azure.Functions.Worker;
using Newtonsoft.Json;
using RegApi.Shared.Models;

namespace EmailSender
{
    public class EmailSenderFunction
    {
        private readonly IEmailSenderService _emailService;

        public EmailSenderFunction(IEmailSenderService emailService)
        {
            _emailService = emailService;
        }

        [Function(nameof(EmailSenderFunction))]
        public async Task Run(
            [ServiceBusTrigger("%EmailQueue%", Connection = "ServiceBusConnectionString")]
            ServiceBusReceivedMessage message,
            ServiceBusMessageActions messageActions)
        {
            var emailSenderModel = JsonConvert.DeserializeObject<EmailSenderModel>(message.Body.ToString());

            await _emailService.SendEmailAsync(new Message(emailSenderModel!));

            // Complete the message
            await messageActions.CompleteMessageAsync(message);
        }
    }
}
