using Domain.Models;

namespace Api.Services.Interfaces
{
    public interface ITimeSeriesService
    {
        Task<List<ScheduledCalcs>> GetScheduledCalcsAsync();
    }
}
