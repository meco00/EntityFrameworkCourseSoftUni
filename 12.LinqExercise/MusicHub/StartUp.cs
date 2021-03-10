namespace MusicHub
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using Data;
    using Initializer;

    public class StartUp
    {
        public static void Main(string[] args)
        {
            MusicHubDbContext context =
                new MusicHubDbContext();

            DbInitializer.ResetDatabase(context);

          


            //Console.WriteLine(ExportSongsAboveDuration(context,4));
        }

        public static string ExportAlbumsInfo(MusicHubDbContext context, int producerId)
        {
            var albumsInfo = context.Albums.Where(x => x.ProducerId == producerId)
                 .ToList()
           .Select(x => new
           {
               AlbumName = x.Name,
               ReleaseDate = x.ReleaseDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture),
               ProducerName = x.Producer.Name,
               AlbumSongs = x.Songs
               .Select(s => new
               {
                   SongName = s.Name,
                   Price = s.Price,
                   SongWriterName = s.Writer.Name
               })
               .OrderByDescending(x => x.SongName)
               .ThenBy(x => x.SongWriterName)
               .ToList(),
               TotalAlbumPrice = x.Price

           })
           .OrderByDescending(x => x.TotalAlbumPrice)
            .ToList();

            var sb = new StringBuilder();

            foreach (var album in albumsInfo)
            {
                //                -AlbumName: Devil's advocate
                //- ReleaseDate: 07 / 21 / 2018
                // - ProducerName: Evgeni Dimitrov
                //-Songs:

                sb.AppendLine($"-AlbumName: {album.AlbumName}")
                    .AppendLine($"-ReleaseDate: {album.ReleaseDate}")
                    .AppendLine($"-ProducerName: {album.ProducerName}")
                    .AppendLine("-Songs:");

                int counter = 1;

                foreach (var song in album.AlbumSongs)
                {


                    sb.AppendLine($"---#{counter++}")
                        .AppendLine($"---SongName: {song.SongName}")
                        .AppendLine($"---Price: {song.Price:f2}")
                        .AppendLine($"---Writer: {song.SongWriterName}");
                }

                sb.AppendLine($"-AlbumPrice: {album.TotalAlbumPrice:f2}");


            }

            return sb.ToString().Trim();


        }

        public static string ExportSongsAboveDuration(MusicHubDbContext context, int duration)
        {
            var songs = context.Songs
                .ToList()
                .Where(x => x.Duration.TotalSeconds > duration)               
                 .Select(x => new
                 {
                     SongName = x.Name,
                     PerformerFullName = x.SongPerformers.Select(x => x.Performer.FirstName + ' ' + x.Performer.LastName).FirstOrDefault(),
                     WriterName = x.Writer.Name,
                     AlbumProducer = x.Album.Producer.Name,
                     Duration = x.Duration


                 })
                 .OrderBy(x => x.SongName)
                 .ThenBy(x => x.WriterName)
                 .ThenBy(x => x.PerformerFullName)
                 .ToList();

            var sb = new StringBuilder();

            int counter = 1;

            foreach (var song in songs)
            {
             

                sb.AppendLine($"-Song #{counter++}")
                    .AppendLine($"---SongName: {song.SongName}")
                    .AppendLine($"---Writer: {song.WriterName}")
                    .AppendLine($"---Performer: {song.PerformerFullName}")
                    .AppendLine($"---AlbumProducer: {song.AlbumProducer}")
                    .AppendLine($"---Duration: {song.Duration.ToString("c",CultureInfo.InvariantCulture)}");
                   


            }

            return sb.ToString().Trim();
        }
    }
}
