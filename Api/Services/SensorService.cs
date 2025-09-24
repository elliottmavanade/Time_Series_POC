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

        public async Task<Tuple<int, List<int>>> GetSensorRelationshipsAsync(int parentId)
        {
            var childIds = new List<int>();
            try
            {
                using (var connection = new SqlConnection(connectionString))
                using (var command = new SqlCommand($"SELECT Sensor_Parent_Id, Sensor_Child_Id FROM Sensor_Calc_Relationships WHERE Sensor_Parent_Id = @ParentId", connection))
                {
                    command.Parameters.AddWithValue("@ParentId", parentId);

                    await connection.OpenAsync();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            int sensorChildId = reader.GetInt32(reader.GetOrdinal("Sensor_Child_Id"));
                            childIds.Add(sensorChildId);
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
            return new Tuple<int, List<int>>(parentId, childIds);
        }

        public async Task<Sensor> GetSensorAsync(int sensorId)
        {
            SensorDTO? sensorDto = null;
            try
            {
                using (var connection = new SqlConnection(connectionString))
                using (var command = new SqlCommand("SELECT Id, Sensor_Model_Id, location, Metadata FROM Sensors WHERE Id = @SensorId", connection))
                {
                    command.Parameters.AddWithValue("@SensorId", sensorId);
                    
                    await connection.OpenAsync();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            sensorDto = new SensorDTO
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Sensor_Model_Id = reader.GetInt32(reader.GetOrdinal("Sensor_Model_Id")),
                                Location = reader.GetString(reader.GetOrdinal("Location")),
                                Metadata = reader.GetString(reader.GetOrdinal("Metadata")) ?? string.Empty
                            };
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
