using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CzadRoom.Models;
using CzadRoom.ViewModels;

namespace CzadRoom.MapperProfiles
{
    public class PublicRoomViewModelProfile : Profile
    {
        public PublicRoomViewModelProfile() {
            CreateMap<PublicRoom, PublicRoomViewModel>()
                .ForMember(dest => dest.ClientCount, output => output.Ignore())
                .ForMember(dest => dest.ClientsName, output => output.Ignore());
        }
    }
}
