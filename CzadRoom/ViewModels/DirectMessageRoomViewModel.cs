using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CzadRoom.ViewModels
{
    public class DirectMessageRoomViewModel
    {
        public string ID { get; set; }
        public UserViewModel Recipent { get; set; }
    }
}
