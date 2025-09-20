using Azure.Messaging.ServiceBus;
using ConsumerFA.Services;
using ConsumerFA.Services.Interfaces;
using Domain.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace ConsumerFA.Functions
{
    public class Consumer
    {
        private readonly ILogger<Consumer> _logger;
        private readonly IApiService _apiService;

        public Consumer(ILogger<Consumer> logger, IApiService apiService)
        {
            _logger = logger;
            _apiService = apiService;
        }

        [Function("ReceiveScheduledCalculations")]
        public async Task Run(
            [ServiceBusTrigger("scheduled-tasks-queue", Connection = "SERVICE_BUS_CONNECTION")]
            ServiceBusReceivedMessage message,
            ServiceBusMessageActions messageActions)
        {
            try
            {
                _logger.LogInformation("Message ID: {id}", message.MessageId);
                _logger.LogInformation("Message Body: {body}", message.Body);
                _logger.LogInformation("Message Content-Type: {contentType}", message.ContentType);

                ScheduledCalcs? scheduledCalculationTask = null;
                try
                {
                    scheduledCalculationTask = JsonSerializer.Deserialize<ScheduledCalcs>(message.Body);
                    if (scheduledCalculationTask == null)
                    {
                        throw new InvalidOperationException("Deserialized ScheduledCalcs is null.");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to deserialize message body to ScheduledCalcs. MessageId: {id}", message.MessageId);
                    // Optionally dead-letter the message
                    await messageActions.DeadLetterMessageAsync(message, null, "DeserializationFailed", ex.Message);
                    return;
                }

                var result = await _apiService.GetSensorRelationships(scheduledCalculationTask.Sensor_Id);

                // Complete the message only after successful processing
                await messageActions.CompleteMessageAsync(message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception while processing message. MessageId: {id}", message.MessageId);
                // Optionally abandon the message for retry
                await messageActions.AbandonMessageAsync(message);
            }
        }
    }
}
