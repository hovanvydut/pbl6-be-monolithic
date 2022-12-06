using Microsoft.AspNetCore.SignalR;

namespace Monolithic.Helpers;

public class NotificationHub : Hub
{
    private readonly string MyBot;
    private readonly IDictionary<string, UserConnected> _connections;
    public NotificationHub(IDictionary<string, UserConnected> connections)
    {
        MyBot = "Bot";
        _connections = connections;
    }

    public Task SendUsersConnected(string room)
    {   
        Console.WriteLine(">> Room: "+room);
        var users = _connections.Values
            .Where(c => c.Room == room)
            .Select(c => c.UserName);

        return Clients.Group(room).SendAsync("UsersInRoom", users);
    }

    public async Task JoinRoom(UserConnected user)
    {
        Console.WriteLine(">> Join Room: " + user.Room + "-"+user.UserName);
        await Groups.AddToGroupAsync(Context.ConnectionId, user.Room);
        _connections[Context.ConnectionId] = user;

        await Clients.Group(user.Room).SendAsync("ReceiveMessage", MyBot,
                                         $"{user.UserName} has joined {user.Room} !");
        await SendUsersConnected(user.Room);
    }

    public async Task SendMessage(string message)
    {
        Console.WriteLine(">> Message: " + message);
        if (_connections.TryGetValue(Context.ConnectionId, out UserConnected user))
        {
            await Clients.Group(user.Room).SendAsync("ReceiveMessage", user.UserName, message);
        }
    }

    public override Task OnDisconnectedAsync(Exception exception)
    {
        Console.WriteLine(">> Exception");
        if (_connections.TryGetValue(Context.ConnectionId, out UserConnected user))
        {
            _connections.Remove(Context.ConnectionId);
            Clients.Group(user.Room).SendAsync("ReceiveMessage", MyBot,
                                                $"{user.UserName} has left !");
            SendUsersConnected(user.Room);
        }
        return base.OnDisconnectedAsync(exception);
    }
}