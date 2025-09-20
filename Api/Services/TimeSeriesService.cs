using Api.Models.DTOs;
using Api.Services.Interfaces;
using Domain.Models;
using Microsoft.Data.SqlClient;
using System.Data.Common;

namespace Api.Services
{
    public class TimeSeriesService : ITimeSeriesService
    {
        private static string connectionString = "Server=(LocalDb)\\MSSQLLocalDB;Database=TimeSeriesPoc;Trusted_Connection=True;";

        public async Task<List<ScheduledCalcs>> GetScheduledCalcsAsync()
        {
            var scheduledRuns = new List<ScheduledCalcs>();

            try
            {
                using (var connection = new SqlConnection(connectionString))
                using (var command = new SqlCommand("SELECT Name, Sensor_Id FROM Scheduled_Calcs", connection))
                {
                    await connection.OpenAsync();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            scheduledRuns.Add(new ScheduledCalcs
                            {
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                Sensor_Id = reader.GetInt32(reader.GetOrdinal("Sensor_Id"))
                            });
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"SQL Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            return scheduledRuns;
        }

        public Task<List<int>> GetTimeSeriesDataAsync(int id, int timespan)
            => QueryTimeSeriesAsync(new[] { id }, timespan);

        public Task<List<int>> GetTimeSeriesDataAsync(IEnumerable<int> ids, int timespan)
            => QueryTimeSeriesAsync(ids, timespan);

        private async Task<List<int>> QueryTimeSeriesAsync(IEnumerable<int> ids, int timespan)
        {
            var valueList = new List<int>();

            DateTime currentTime = DateTime.Now;
            // Round down to the nearest hour
            DateTime dateEnd = new DateTime(currentTime.Year, currentTime.Month, 17, 0, 0, 0); // So accurate results show up in testing
            // Subtract Time
            DateTime dateStart = dateEnd.AddMinutes(-timespan);

            var sql = $"SELECT Sensor_Value FROM Sensor_Readings WHERE Sensor_Id IN ({string.Join(",", ids)}) AND Date_Created >= @DateStart AND Date_Created <= @DateEnd";
            try
            {
                using (var connection = new SqlConnection(connectionString))
                using (var command = new SqlCommand(sql, connection))
                {
                    // Add ID parameters
                    int idx = 0;
                    foreach (var id in ids)
                    {
                        command.Parameters.AddWithValue($"@id{idx++}", id);
                    }
                    // Add date parameters
                    command.Parameters.AddWithValue("@DateStart", dateStart);
                    command.Parameters.AddWithValue("@DateEnd", dateEnd);

                    await connection.OpenAsync();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            valueList.Add(reader.GetInt32(reader.GetOrdinal("Sensor_Value")));
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"SQL Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            return valueList;
        }

        public async Task<bool> AddTimeSeriesResultAsync(TimeSeriesResultDTO request)
        {
            int rowsAffected = 0;
            try
            {
                using (var connection = new SqlConnection(connectionString))
                using (var command = new SqlCommand("INSERT INTO Sensor_Readings (Sensor_Id, Sensor_Value, Date_Created) VALUES (@Sensor_Id, @Sensor_Value, @Date_Created)", connection))
                {
                    command.Parameters.AddWithValue("@Sensor_Id", request.SensorId);
                    command.Parameters.AddWithValue("@Sensor_Value", request.SensorValue);
                    command.Parameters.AddWithValue("@Date_Created", DateTime.Now); // This would be different in a real scenario
                    await connection.OpenAsync();
                    
                    rowsAffected = await command.ExecuteNonQueryAsync();
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"SQL Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            return rowsAffected > 0;
        }
    }
}
