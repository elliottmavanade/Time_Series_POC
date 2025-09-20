using ConsumerFA.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsumerFA.Services
{
    public class CalculatorService : ICalculatorService
    {
        private readonly ILogger<ApiService> _logger;
        public CalculatorService(ILogger<ApiService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public int Calculate(List<int> data, string aggregation)
        {
            if (data == null || data.Count == 0)
            {
                _logger.LogWarning("No data provided for calculation.");
                return 0; // or throw an exception based on your requirements
            }
            return aggregation.ToLower() switch
            {
                "sum" => data.Sum(),
                "avg" => (int)data.Average(),
                "max" => data.Max(),
                "min" => data.Min(),
                _ => throw new ArgumentException($"Unknown aggregation type: {aggregation}")
            };
        }
    }
}
