using Api.Services.Interfaces;
using Domain.Models;
using Microsoft.Data.SqlClient;

namespace Api.Services
{
    public class DashboardService : IDashboardService
    {
        private static string connectionString = "Server=(LocalDb)\\MSSQLLocalDB;Database=TimeSeriesPoc;Trusted_Connection=True;";
        private readonly SqlConnection _connection;

        public DashboardService()
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

        public async Task<Dictionary<int, List<int>>> GetSensorRelationshipsAsync(int parentId)
        {
            var sensorRelationships = new Dictionary<int, List<int>>();
            try
            {
                SqlCommand command = new SqlCommand($"SELECT Sensor_Parent_Id, Sensor_Child_Id FROM Sensor_Calc_Relationships WHERE Sensor_Parent_Id = {parentId}", _connection);
                SqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    int sensorParentId = (int)reader["Sensor_Parent_Id"];
                    int sensorChildId = (int)reader["Sensor_Child_Id"];
                    if (!sensorRelationships.ContainsKey(sensorParentId))
                    {
                        sensorRelationships[sensorParentId] = new List<int>();
                    }
                    sensorRelationships[parentId].Add(sensorChildId);
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
            return sensorRelationships;
        }
    }
}
