using AutoMapper;
using CzadRoom.Models;
using CzadRoom.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CzadRoom.MapperProfiles
{
    public class RoomViewModelProfile : Profile
    {
        public RoomViewModelProfile() {
            CreateMap<Room, RoomViewModel>()
                .ForMember(dest => dest.HasPassword, output => output.MapFrom(src => !string.IsNullOrEmpty(src.Password)))
                .ForMember(dest => dest.ClientCount, output => output.Ignore())
                .ForMember(dest => dest.UsersInRoom, output => output.Ignore());
        }
    }
}
