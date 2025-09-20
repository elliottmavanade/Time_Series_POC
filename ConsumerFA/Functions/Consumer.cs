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
        private readonly ICalculatorService _calculatorService;

        public Consumer(ILogger<Consumer> logger, IApiService apiService, ICalculatorService calculatorService)
        {
            _logger = logger;
            _apiService = apiService;
            _calculatorService = calculatorService;
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

                // Retrieve sensor relationships to find out what calculations to perform
                var relationship = await _apiService.GetSensorRelationships(scheduledCalculationTask.Sensor_Id);

                // Retrieve parent sensor details to get aggregation and timespan
                var sensor = await _apiService.GetSensor(scheduledCalculationTask.Sensor_Id);

                // Retrieve time series data required for calculation
                var timeSeriesData = await _apiService.GetTimeSeriesData(sensor.Id, relationship);

                // Complete the message only after successful processing
                await messageActions.CompleteMessageAsync(message);
            }
            catch(InvalidDataException ex)
            {
                _logger.LogError(ex, "Invalid data encountered while processing message. MessageId: {id}", message.MessageId);
                // Optionally dead-letter the message
                await messageActions.DeadLetterMessageAsync(message, null, "InvalidData", ex.Message);
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
