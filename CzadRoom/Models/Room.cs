using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Driver;

namespace CzadRoom.Models {
    public class Room {
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        public string ID { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public DateTime CreationDate { get; set; }

        public string OwnerID { get; set; }
        public HashSet<string> UsersIDWithAccess { get; set; }
        public HashSet<string> CurrentUsersID { get; set; }

        public Room() {
            ID = Guid.NewGuid().ToString();
            CreationDate = DateTime.Now;
            UsersIDWithAccess = new HashSet<string>();
            CurrentUsersID = new HashSet<string>();
        }
    }
}