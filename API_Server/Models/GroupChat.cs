﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
namespace API_Server.Models
{
    public class GroupChat
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Sender { get; set; }
        public string Receiver { get; set; }
        public string GroupId { get; set; }
        public string Content { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
