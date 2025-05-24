namespace Domain.Hubs.Connection
{
    public interface IConnectionManager
    {
        public Dictionary<string, string> Connections { get; }

        public List<string> GetUserConnections(string userId);

        public void AddConnection(string connectionId, string userId);

        public void RemoveConnection(string connectionId);
    }
}