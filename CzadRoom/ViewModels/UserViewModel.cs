using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CzadRoom.ViewModels
{
    public class UserViewModel
    {
        public string Nickname { get; set; }
        public string Username { get; set; }
        [Display(Name = "Avatar")]
        public string AvatarName { get; set; }
    }
}
