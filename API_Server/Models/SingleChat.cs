using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

public class SingleChat
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [BsonElement("sender")]
    public string Sender { get; set; }

    [BsonElement("receiver")]
    public string Receiver { get; set; }

    [BsonElement("message")]
    public string Message { get; set; }

    [BsonElement("timestamp")]
    public DateTime Timestamp { get; set; }
}