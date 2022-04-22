using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.DTOs;
using DatingApp.Entities;
using DatingApp.Extensions;

namespace DatingApp.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            // Telling mapper to map to sourcae to destination object
            // <Source => Destination>
            CreateMap<UserProfile, UserProfileDto>()
                // Configuring automapper so that it pick property from child class and assign it to parent class
                .ForMember(destination => destination.MainPhotoUrl, 
                options => options.MapFrom(source => source.Photos.FirstOrDefault(p => p.IsMain).Url))
                .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.DateOfBirth.CalculateAge()));
            CreateMap<Photo, PhotoDto>();            
            CreateMap<UserProfileUpdateDto, UserProfile>();
        }
    }
}