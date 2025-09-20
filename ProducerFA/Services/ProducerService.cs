using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProducerFA.Services
{
    public class ProducerService : Interfaces.IProducerService
    {
        private readonly ILogger<ProducerService> _logger;
        private readonly ServiceBusClient _client;

        private const string QueueName = "scheduled-tasks-queue";
        private const string fullyQualifiedNamespace = "Endpoint=sb://localhost;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=SAS_KEY_VALUE;UseDevelopmentEmulator=true;";

        public ProducerService(ILogger<ProducerService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _client = new ServiceBusClient(fullyQualifiedNamespace);
        }

        public async Task SendMessageAsync(string message)
        {
            try
            {
                // Could implement batches too
                ServiceBusSender sender = _client.CreateSender(QueueName);
                ServiceBusMessage busMessage = new ServiceBusMessage(message);
                await sender.SendMessageAsync(busMessage);
                _logger.LogInformation($"Message sent to queue: {message}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending message to queue");
                throw;
            }
        }
    }
}
