using API_Server.Models;
using API_Server.Services;
using MongoDB.Driver;
using NetStudy.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

public class SingleChatService
{
    private readonly IMongoCollection<SingleChat> _chats;
    
    public SingleChatService(MongoDbService mongoDbService)
    {
        var database = mongoDbService.GetDatabase();
        _chats = database.GetCollection<SingleChat>("SingleChats");
    }
    
    public async Task<List<SingleChat>> GetChatHistory(string user1, string user2)
    {
        var filter = Builders<SingleChat>.Filter.Or(
            Builders<SingleChat>.Filter.And(
                Builders<SingleChat>.Filter.Eq(chat => chat.Sender, user1),
                Builders<SingleChat>.Filter.Eq(chat => chat.Receiver, user2)
            ),
            Builders<SingleChat>.Filter.And(
                Builders<SingleChat>.Filter.Eq(chat => chat.Sender, user2),
                Builders<SingleChat>.Filter.Eq(chat => chat.Receiver, user1)
            )
        );
        return await _chats.Find(filter).ToListAsync();
    }
    
    public async Task SendMessage(SingleChat chat)
    {
        await _chats.InsertOneAsync(chat);
    }
}