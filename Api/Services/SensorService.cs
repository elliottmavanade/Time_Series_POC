using Api.Models.DTOs;
using Api.Services.Interfaces;
using Domain.Enums;
using Domain.Models;
using Microsoft.Data.SqlClient;
using System.Data.Common;

namespace Api.Services
{
    public class SensorService : ISensorService
    {
        private static string connectionString = "Server=(LocalDb)\\MSSQLLocalDB;Database=TimeSeriesPoc;Trusted_Connection=True;";
        private readonly SqlConnection _connection;

        public SensorService()
        {
            _connection = new SqlConnection(connectionString);
            _connection.Open();
        }

        public async Task<Tuple<int, List<int>>> GetSensorRelationshipsAsync(int parentId)
        {
            var childIds = new List<int>();
            try
            {
                SqlCommand command = new SqlCommand($"SELECT Sensor_Parent_Id, Sensor_Child_Id FROM Sensor_Calc_Relationships WHERE Sensor_Parent_Id = {parentId}", _connection);
                SqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    int sensorChildId = (int)reader["Sensor_Child_Id"];
                    childIds.Add(sensorChildId);
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
            return new Tuple<int, List<int>>(parentId, childIds);
        }

        public async Task<Sensor?> GetSensorAsync(int sensorId)
        {
            SensorDTO? sensorDto = null;
            try
            {
                SqlCommand command = new SqlCommand($"SELECT Id, Sensor_Model_Id, location, Metadata FROM Sensors WHERE Id = {sensorId}", _connection);
                SqlDataReader reader = await command.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    sensorDto = new SensorDTO
                    {
                        Id = (int)reader["Id"],
                        Sensor_Model_Id = (int)reader["Sensor_Model_Id"],
                        Location = reader["Location"].ToString() ?? string.Empty,
                        Metadata = reader["Metadata"].ToString() ?? string.Empty
                    };
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
                _connection.Close(); //Should you be closign everytime particuarly if you have several jobs happenig?
            }

            if (sensorDto == null) return null;

            var metadata = sensorDto?.Metadata != null ? System.Text.Json.JsonSerializer.Deserialize<SensorMetadata>(sensorDto.Metadata) : null;
            
            return new Sensor
            {
                Id = sensorDto!.Id,
                Sensor_Model_Id = sensorDto!.Sensor_Model_Id,
                Location = sensorDto.Location,
                Aggregation = metadata != null && Enum.TryParse<AggregationType>(metadata.Aggregation, out var agg) ? agg : null,
                Timespan = metadata != null ? metadata.Timespan : null
            };
        }
    }
}
