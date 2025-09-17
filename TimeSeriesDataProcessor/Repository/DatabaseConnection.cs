using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeSeriesDataProcessor.Repository
{
    public class DatabaseConnection
    {
        private static string connectionString = "Server=(LocalDb)\\MSSQLLocalDB;Database=TimeSeriesPoc;Trusted_Connection=True;";
        private readonly SqlConnection _connection;

        public DatabaseConnection()
        {
            _connection = new SqlConnection(connectionString);
            _connection.Open();
        }

        public async Task<List<ScheduledRuns>> GetScheduledRunsAsync()
        {
            var scheduledRuns = new List<ScheduledRuns>();
           // Make sure no sql injection can occur
        }
    }
}
