using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.DTOs;
using DatingApp.Entities;

namespace DatingApp.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            // Telling mapper to map to sourcae to destination object
            // <Source => Destination>
            CreateMap<UserProfile, UserProfileDto>();
            CreateMap<Photo, PhotoDto>();
        }
    }
}