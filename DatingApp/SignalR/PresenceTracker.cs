namespace DatingApp.SignalR
{
    public class PresenceTracker
    {
        // here we are storing all online users presence tag in dictionary which is store in memory
        // so if this app has a lot of users register, there will be a big problem which is buffer overflow
        // there are alternate for this task which is storing users presence in a database or reddis db
        private static readonly Dictionary<string, List<string>> _onlineUsers =
            new Dictionary<string, List<string>>();


        // mainting a list of online user in memory by using dictionary
        public Task<bool> UserConnected(string username, string connectionId)
        {
            bool isOnline = false;
            // lock dictionary till below code execute this will concurrent operation on dictionary
            lock (_onlineUsers)
            {
                if (_onlineUsers.ContainsKey(username))
                {
                    _onlineUsers[username].Add(connectionId);
                }
                else
                {
                    _onlineUsers.Add(username, new List<string> { connectionId });
                    isOnline = true;
                }

                return Task.FromResult(isOnline);
            }
        }

        public Task<bool> UserDisconnected(string username, string connectionId)
        {
            bool isOffline = false;
            lock (_onlineUsers)
            {
                if (!_onlineUsers.ContainsKey(username)) return Task.FromResult(isOffline);

                _onlineUsers[username].Remove(connectionId);
                if (_onlineUsers[username].Count == 0)
                {
                    _onlineUsers.Remove(username);
                    isOffline = true;
                }
            }

            return Task.FromResult(isOffline);
        }

        public Task<string[]> GetOnlineUsers()
        {
            string[] onlineUsers;
            lock (_onlineUsers)
            {
                onlineUsers = _onlineUsers.OrderBy(key => key.Key).Select(key => key.Key).ToArray();
            }

            return Task.FromResult(onlineUsers);
        }

        public Task<List<string>> GetConnectionForUser(string username)
        {
            List<string> connectionIds;
            lock (_onlineUsers)
            {
                connectionIds = _onlineUsers.GetValueOrDefault(username);
            }

            return Task.FromResult(connectionIds);
        }
    }
}