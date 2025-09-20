using Api.Services.Interfaces;
using Domain.Models;
using Microsoft.Data.SqlClient;

namespace Api.Services
{
    public class DashboardService : IDashboardService
    {
        private static string connectionString = "Server=(LocalDb)\\MSSQLLocalDB;Database=TimeSeriesPoc;Trusted_Connection=True;";
   
    }
}
