using API_Server.Models;
using API_Server.Services;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

public class ChatHub : Hub
{
    private readonly SingleChatService _chatService;

    public ChatHub(SingleChatService chatService)
    {
        _chatService = chatService;
    }

    public async Task SendMessage(string user, string receiver, string message)
    {
        // Lưu tin nhắn vào MongoDB
        var chat = new SingleChat
        {
            Sender = user,
            Receiver = receiver,
            Message = message,
            Timestamp = DateTime.UtcNow
        };
        await _chatService.SendMessage(chat);

        // Gửi tin nhắn đến tất cả các client
        //await Clients.All.SendAsync("ReceiveMessage", user, message);
        await Clients.Users(receiver).SendAsync("ReceiveMessage", user, message);
        await Clients.Users(user).SendAsync("ReceiveMessage", user, message);
    }
}