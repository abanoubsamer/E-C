namespace Domain.Hubs.Connection
{
    public class ConnectionManager : IConnectionManager
    {
        private readonly Dictionary<string, string> _connections = new Dictionary<string, string>();

        public Dictionary<string, string> Connections => _connections;

        public void AddConnection(string connectionId, string userId)
        {
            if (!_connections.ContainsKey(connectionId))
            {
                _connections[connectionId] = userId;
            }
        }

        public void RemoveConnection(string connectionId)
        {
            if (_connections.ContainsKey(connectionId))
            {
                _connections.Remove(connectionId);
            }
        }

        public List<string> GetUserConnections(string userId)
        {
            return _connections
                .Where(c => c.Value == userId)
                .Select(c => c.Key)
                .ToList();
        }
    }
}