using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CzadRoom.ViewModels
{
    public class PublicRoomViewModel
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public int ClientCount { get; set; }
        public IEnumerable<string> ClientsName { get; set; }
    }
}
