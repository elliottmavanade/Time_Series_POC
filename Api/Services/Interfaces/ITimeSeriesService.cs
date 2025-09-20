using Domain.Models;

namespace Api.Services.Interfaces
{
    public interface ITimeSeriesService
    {
        Task<List<ScheduledCalcs>> GetScheduledCalcsAsync();

        Task<List<ScheduledCalcs>> GetTimeSeriesDataAsync(IEnumerable<int> ids, int timespan);

        Task<List<int>> GetTimeSeriesDataAsync(int id, int timespan);

        Task<List<int>> QueryTimeSeriesAsync(IEnumerable<int> ids, int timespan);
    }
}
