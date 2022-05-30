using DatingApp.Extensions;
using DatingApp.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace DatingApp.SignalR
{
    public class PresenceHub : Hub
    {
        // here we are storing all online users presence tag in dictionary which is store in memory
        // so if this app has a lot of users register, there will be a big problem which is buffer overflow
        // there are alternate for this task which is storing users presence in a database or reddis db
        private readonly PresenceTracker _presenceTracker;
        private readonly IUserProfileRepository _userProfileRepository;
        public PresenceHub(PresenceTracker presenceTracker, IUserProfileRepository userProfileRepository)
        {
            _userProfileRepository = userProfileRepository;
            _presenceTracker = presenceTracker;
        }

        public override async Task OnConnectedAsync()
        {
            var username = (await _userProfileRepository.GetUserByAppIdAsync(Context.User.GetAppUserId())).KnownAs;
            await _presenceTracker.UserConnected(username, Context.ConnectionId);
            await Clients.Others.SendAsync("Online", username);

            var currentUsers = await _presenceTracker.GetOnlineUsers();
            await Clients.All.SendAsync("GetOnlineUsers", currentUsers);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var username = (await _userProfileRepository.GetUserByAppIdAsync(Context.User.GetAppUserId())).KnownAs;
            await _presenceTracker.UserDisconnected(username, Context.ConnectionId);
            await Clients.Others.SendAsync("Offline", username);

            await base.OnDisconnectedAsync(exception);


            var currentUsers = await _presenceTracker.GetOnlineUsers();
            await Clients.All.SendAsync("GetOnlineUsers", currentUsers);
        }
    }
}