using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace CzadRoom.Models
{
    public class DirectMessage
    {
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        public string ID { get; set; }
        public string RoomID { get; set; }
        public string FromID { get; set; }
        public string Content { get; set; }
        public bool Read { get; set; }
        public DateTime Date { get; set; }

        public DirectMessage() {
            Date = DateTime.Now;
            Read = false;
        }
    }
}
