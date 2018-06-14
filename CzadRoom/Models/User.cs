using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CzadRoom.Models
{
    public class User
    {
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        public string ID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public DateTime CreationDate { get; set; }
        public string Email { get; set; }
        public HashSet<string> CurrentRoomsID { get; set; }
        public HashSet<string> FriendsID { get; set; }
        public string AvatarName { get; set; }

        public User() {
            CreationDate = DateTime.Now;
            CurrentRoomsID = new HashSet<string>();
            FriendsID = new HashSet<string>();
        }
    }
}
