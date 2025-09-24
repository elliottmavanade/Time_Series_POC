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
            [ServiceBusTrigger("scheduled-tasks-queue", Connection = "SERVICE_BUS_CONNECTION")] //TODO: Add to appsettings
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
                var relationshipsTask = _apiService.GetSensorRelationships(scheduledCalculationTask.Sensor_Id);
               
                // Retrieve parent sensor details to get aggregation and timespan
                var sensorTask = _apiService.GetSensor(scheduledCalculationTask.Sensor_Id);

                // Run both tasks in parallel
                await Task.WhenAll(relationshipsTask, sensorTask);

                var relationships = await relationshipsTask;
                var sensor = await sensorTask;

                // Retrieve time series data required for calculation
                var timeSeriesData = await _apiService.GetTimeSeriesData(relationships.Item2, (int)sensor.Timespan!);

                // Perform the calculation
                var calculationResult = _calculatorService.Calculate(timeSeriesData, sensor.Aggregation.ToString());

                // Add the aggregated result into the Time Series table
                await _apiService.AddCalculatedResult(scheduledCalculationTask.Sensor_Id, calculationResult);

                _logger.LogInformation($"The calculation result for: {scheduledCalculationTask.Sensor_Id}, Name: {scheduledCalculationTask.Name} over {sensor.Timespan} is: {calculationResult} ");
                
                // Complete the message only after successful processing
                await messageActions.CompleteMessageAsync(message);
            }
            catch(InvalidOperationException ex)
            {
                _logger.LogError(ex, "Invalid operation while processing message. MessageId: {id}", message.MessageId);
                // Dead-letter the message if there are invalid operations
                await messageActions.DeadLetterMessageAsync(message, null, "InvalidOperation", ex.Message);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Argument error while processing message. MessageId: {id}", message.MessageId);
                // Dead-letter the message if there are argument issues
                await messageActions.DeadLetterMessageAsync(message, null, "Invalid Aggregation type", ex.Message);
            }
            catch (AggregateException ex)
            {
                foreach (var inner in ex.InnerExceptions)
                {
                    _logger.LogError(inner, "Error in parallel API calls.");
                }
                // Dead-letter the message if API calls fail
                await messageActions.DeadLetterMessageAsync(message, null, "Invalid Data", ex.Message);
            }
            catch (InvalidDataException ex)
            {
                _logger.LogError(ex, "Invalid data encountered while processing message. MessageId: {id}", message.MessageId);
                // Dead-letter the message if data is invalid
                await messageActions.DeadLetterMessageAsync(message, null, "Invalid Data", ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception while processing message. MessageId: {id}", message.MessageId);
                // Abandon the message to make it available for reprocessing
                await messageActions.AbandonMessageAsync(message);
            }
        }
    }
}
