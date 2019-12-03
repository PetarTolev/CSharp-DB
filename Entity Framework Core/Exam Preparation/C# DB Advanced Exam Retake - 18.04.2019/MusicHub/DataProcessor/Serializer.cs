using MusicHub.DataProcessor.ExportDtos;
using Newtonsoft.Json;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Formatting = Newtonsoft.Json.Formatting;

namespace MusicHub.DataProcessor
{

    using Data;

    public class Serializer
    {
        public static string ExportAlbumsInfo(MusicHubDbContext context, int producerId)
        {
            var albums = context.Albums
                .Where(a => a.Producer.Id == producerId)
                .Select(a =>
                    new
                    {
                        AlbumName = a.Name,
                        ReleaseDate = a.ReleaseDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture),
                        ProducerName = a.Producer.Name,
                        Songs = a.Songs.Select(s =>
                                new
                                {
                                    SongName = s.Name,
                                    Price = s.Price.ToString("F2"),
                                    Writer = s.Writer.Name
                                })
                            .OrderByDescending(s => s.SongName)
                            .ThenBy(s => s.Writer)
                            .ToArray(),
                        AlbumPrice = a.Songs.Sum(s => s.Price).ToString("F2")
                    })
                .OrderByDescending(a => decimal.Parse(a.AlbumPrice))
                .ToArray();

            var json = JsonConvert.SerializeObject(albums, Formatting.Indented);
            return json;
        }

        public static string ExportSongsAboveDuration(MusicHubDbContext context, int duration)
        {
            var songs = context.Songs
                .Where(s => s.Duration.TotalSeconds > duration)
                .Select(s =>
                    new SongAboveDurationDto
                    {
                        SongName = s.Name,
                        Writer = s.Writer.Name,
                        Performer = s.SongPerformers.Select(sp => $"{sp.Performer.FirstName} {sp.Performer.LastName}").FirstOrDefault(),
                        AlbumProducer = s.Album.Producer.Name,
                        Duration = s.Duration.ToString("c")
                    })
                .OrderBy(s => s.SongName)
                .ThenBy(s => s.Writer)
                .ThenBy(s => s.Performer)
                .ToArray();

            var serializer = new XmlSerializer(typeof(SongAboveDurationDto[]), 
                new XmlRootAttribute("Songs"));

            var sb = new StringBuilder();
            var namespaces = new XmlSerializerNamespaces(new[] { new XmlQualifiedName("", "") });
	
            serializer.Serialize(new StringWriter(sb), songs, namespaces);
	
            return sb.ToString().TrimEnd();
        }
    }
}