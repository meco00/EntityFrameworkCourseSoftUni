using AutoMapper;
using AutoMapper.QueryableExtensions;
using AutoMappingLab.MappingProfiles;
using AutoMappingLab.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Text.Json;

namespace AutoMappingLab
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;


            var context = new MusicXContext();

            var config = new MapperConfiguration(cfg=>
              cfg.AddProfile(new SongProfile())
              );

            var mapper = config.CreateMapper();

            var song = context.Songs.FirstOrDefault();

            var songDTO = mapper.Map<SongDTO>(song);


            //var songReverse = mapper.Map<SongDTO, Song>(songDTO);

            var songDTO2 = context.Songs
                .Where(x => x.Source.Name.StartsWith("Us"))
                .ProjectTo<SongDTO>(config).ToList();





            Console.WriteLine(JsonConvert.SerializeObject(songDTO, Formatting.Indented));



        }

        public class SongDTO
        {

            public string SongArtists { get; set; }

            public string Name { get; set; }

            public string SourceName { get; set; }

            public DateTime CreatedOn { get; set; }

            public DateTime? ModifiedOn { get; set; }
        }
    }
}
