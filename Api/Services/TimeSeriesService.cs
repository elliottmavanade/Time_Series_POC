using Api.Services.Interfaces;
using Domain.Models;
using Microsoft.Data.SqlClient;
using System.Data.Common;

namespace Api.Services
{
    public class TimeSeriesService : ITimeSeriesService
    {
        private static string connectionString = "Server=(LocalDb)\\MSSQLLocalDB;Database=TimeSeriesPoc;Trusted_Connection=True;";
        private readonly SqlConnection _connection;

        public TimeSeriesService()
        {
            _connection = new SqlConnection(connectionString);
            _connection.Open();
        }

        public async Task<List<ScheduledCalcs>> GetScheduledCalcsAsync()
        {
            var scheduledRuns = new List<ScheduledCalcs>();

            try
            {
                SqlCommand command = new SqlCommand("SELECT Name, Sensor_Id FROM Scheduled_Calcs", _connection);
                SqlDataReader reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    var scheduledRun = new ScheduledCalcs
                    {
                        Name = (string)reader["Name"],
                        Sensor_Id = (int)reader["Sensor_Id"]
                    };
                    scheduledRuns.Add(scheduledRun);
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
            finally
            {
                _connection.Close();
            }
            return scheduledRuns;
        }

    }
}
