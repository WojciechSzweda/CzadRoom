using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CzadRoom.Models
{
    public class DirectMessageRoom
    {
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        public string ID { get; set; }
        public DateTime CreationDate { get; set; }

        public HashSet<string> Users { get; set; }

        public DirectMessageRoom(string userId, string friendId) {
            CreationDate = DateTime.Now;
            Users = new HashSet<string>() { userId, friendId };
        }
    }
}
