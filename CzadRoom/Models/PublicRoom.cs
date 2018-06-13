using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace CzadRoom.Models
{
    public class PublicRoom
    {
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        public string ID { get; set; }
        public string Name { get; set; }
    }
}
