using AutoMapper;
using CzadRoom.Models;
using CzadRoom.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CzadRoom.MapperProfiles {
    public class RegisterUserProfile : Profile {
        public RegisterUserProfile() {
            CreateMap<UserRegisterViewModel, User>();
        }
    }
}
