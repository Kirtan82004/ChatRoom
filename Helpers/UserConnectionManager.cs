namespace Connectify.Helpers
{
    public class UserConnectionManager
    {
        private static readonly Dictionary<string, string>
            _connections = new();

        public void AddUser(string connectionId, string userName)
        {
            _connections[connectionId] = userName;
        }

        public void RemoveUser(string connectionId)
        {
            if (_connections.ContainsKey(connectionId))
            {
                _connections.Remove(connectionId);
            }
        }

        public string GetUser(string connectionId)
        {
            return _connections[connectionId];
        }

        public int OnlineUsersCount()
        {
            return _connections.Count;
        }

        public List<string> GetOnlineUsers()
        {
            return _connections.Values
                .Distinct()
                .ToList();
        }
    }
}