using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CzadRoom.ViewModels {
    public class UserRegisterViewModel {
        [Required]
        [MaxLength(64)]
        [Remote(action: "IsUsernameUnique", controller: "Account", ErrorMessage = "Username already taken")]
        public string Username { get; set; }
        [Required]
        [EmailAddress]
        [Remote(action: "IsEmailUnique", controller: "Account", ErrorMessage = "Email already in use")]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [Compare("Password", ErrorMessage = "Passwords are not the same")]
        public string RepeatPassword { get; set; }
        public string Nickname { get; set; }
    }
}
