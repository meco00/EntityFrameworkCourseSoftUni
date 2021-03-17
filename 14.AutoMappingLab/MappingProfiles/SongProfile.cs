using AutoMapper;
using AutoMappingLab.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static AutoMappingLab.Program;

namespace AutoMappingLab.MappingProfiles
{
   public class SongProfile:Profile
    {
        public SongProfile()
        {
            CreateMap<Song, SongDTO>().ForMember(dto => dto.SongArtists,
              opt => opt
              .MapFrom(src => string.Join(", ", src.SongArtists.Select(x => x.Artist.Name))))
              .ReverseMap();
        }
    }
}
