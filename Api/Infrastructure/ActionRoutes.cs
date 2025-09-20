namespace Api.Infrastructure
{
    public class ActionRoutes
    {
        public const string Empty = "";
        
        // Client
        public const string AddClient = "/AddClient";

        // Dashboard
        public const string GetScheduledTasks = "GetScheduledTasks";
        public const string GetSensorRelationships = "GetSensorRelationships/{parentId}";
    }
}
