using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CzadRoom.Models;
using CzadRoom.ViewModels;

namespace CzadRoom.MapperProfiles
{
    public class DirectMessageViewModelProfile : Profile {
        public DirectMessageViewModelProfile() {
            CreateMap<DirectMessage, DirectMessageViewModel>()
                .ForMember(dest => dest.From, output => output.Ignore());
        }
    }
}
