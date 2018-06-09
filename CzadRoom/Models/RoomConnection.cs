using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CzadRoom.Models
{
    public struct RoomConnection
    {
        public string RoomID { get; set; }
        public string UserID { get; set; }

    }
}
