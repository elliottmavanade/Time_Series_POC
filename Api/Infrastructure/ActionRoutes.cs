namespace Api.Infrastructure
{
    public class ActionRoutes
    {
        // Client
        public const string AddClient = "AddClient";

        // Dashboard
        public const string GetDashboardData = "GetDashboardData";

        // TimeSeries
        public const string GetScheduledCalculations = "GetScheduledTasks";
        public const string GetTimeSeriesDataById = "GetTimeSeriesData/{sensor_id}/{timespan}";
        public const string GetTimeSeriesDataByIds = "GetTimeSeriesData";
        public const string AddTimeSeriesResult = "AddTimeSeriesResult";

        // Sensor
        public const string GetSensorRelationships = "GetSensorRelationships/{parentId}";
        public const string GetSensor = "GetSensor/{sensorId}";
    }
}
