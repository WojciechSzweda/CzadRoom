﻿using CzadRoom.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CzadRoom.Contexts
{
    public interface IMongoDbContext
    {
        IMongoCollection<User> Users { get; }
        IMongoCollection<ChatRoom> Rooms { get; }
        IMongoCollection<ChatMessage> ChatMessages { get; }
        IMongoCollection<DirectMessageRoom> DirectMessagesRooms { get; }
        IMongoCollection<DirectMessage> DirectMessages { get; }
        IMongoCollection<PublicRoom> PublicRooms { get; }
    }
}
