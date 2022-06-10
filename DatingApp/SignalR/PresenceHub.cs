using DatingApp.Extensions;
using DatingApp.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace DatingApp.SignalR
{
    public class PresenceHub : Hub
    {
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
            var isOnline = await _presenceTracker.UserConnected(username, Context.ConnectionId);
            if(isOnline)
                await Clients.Others.SendAsync("Online", username);

            var currentUsers = await _presenceTracker.GetOnlineUsers();
            // only caller means sender known whose online and whose not
            await Clients.Caller.SendAsync("GetOnlineUsers", currentUsers);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var username = (await _userProfileRepository.GetUserByAppIdAsync(Context.User.GetAppUserId())).KnownAs;
            var isOffline = await _presenceTracker.UserDisconnected(username, Context.ConnectionId);
            if (isOffline)
                await Clients.Others.SendAsync("Offline", username);

            await base.OnDisconnectedAsync(exception);            
        }
    }
}