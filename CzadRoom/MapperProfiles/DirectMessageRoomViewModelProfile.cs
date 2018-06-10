using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CzadRoom.Models;
using CzadRoom.ViewModels;

namespace CzadRoom.MapperProfiles
{
    public class DirectMessageRoomViewModelProfile : Profile {
        public DirectMessageRoomViewModelProfile() {
            CreateMap<DirectMessageRoom, DirectMessageRoomViewModel>()
                .ForMember(dest => dest.Recipent, output => output.Ignore());
        }
    }
}
