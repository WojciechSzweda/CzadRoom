using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CzadRoom.ViewModels
{
    public class RoomJoinViewModel
    {
        public string ID { get; set; }
        public string Name { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
