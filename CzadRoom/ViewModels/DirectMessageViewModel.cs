using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CzadRoom.ViewModels
{
    public class DirectMessageViewModel
    {
        public string Content { get; set; }
        public DateTime Date { get; set; }
        public UserViewModel From { get; set; }
        public bool Read { get; set; }
    }
}
