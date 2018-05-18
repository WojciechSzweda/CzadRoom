using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CzadRoom.Models
{
    public class Room
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }

        public Room() {
            ID = Guid.NewGuid().ToString();
        }
    }
}
