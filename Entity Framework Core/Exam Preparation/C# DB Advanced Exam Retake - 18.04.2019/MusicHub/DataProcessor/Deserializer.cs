using System.IO;
using System.Security.Claims;
using System.Threading.Tasks.Sources;
using System.Xml.Serialization;
using MusicHub.Data.Models.Enums;

namespace MusicHub.DataProcessor
{
    using Data;
    using Data.Models;
    using ImportDtos;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data";

        private const string SuccessfullyImportedWriter 
            = "Imported {0}";
        private const string SuccessfullyImportedProducerWithPhone 
            = "Imported {0} with phone: {1} produces {2} albums";
        private const string SuccessfullyImportedProducerWithNoPhone
            = "Imported {0} with no phone number produces {1} albums";
        private const string SuccessfullyImportedSong 
            = "Imported {0} ({1} genre) with duration {2}";
        private const string SuccessfullyImportedPerformer
            = "Imported {0} ({1} songs)";

        public static string ImportWriters(MusicHubDbContext context, string jsonString)
        {
            var writersDto = JsonConvert.DeserializeObject<ImportWriterDto[]>(jsonString)
                .ToArray();
            var validWriters = new List<Writer>();

            var sb = new StringBuilder();

            foreach (var writerDto in writersDto)
            {
                if (!IsValid(writerDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var writer = AutoMapper.Mapper.Map<Writer>(writerDto);
                validWriters.Add(writer);

                sb.AppendLine(string.Format(SuccessfullyImportedWriter, writer.Name));
            }

	        context.Writers.AddRange(validWriters);
	        context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportProducersAlbums(MusicHubDbContext context, string jsonString)
        {
            var producersDto = JsonConvert.DeserializeObject<ImportProducerDto[]>(jsonString)
                .ToArray();
            var validProducers = new List<Producer>();

            var sb = new StringBuilder();

            foreach (var producerDto in producersDto)
            {
                if (!IsValid(producerDto) || !producerDto.Albums.All(IsValid))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var producer = AutoMapper.Mapper.Map<Producer>(producerDto);
                validProducers.Add(producer);

                if (producer.PhoneNumber == null)
                {
                    sb.AppendLine(
                        string.Format(SuccessfullyImportedProducerWithNoPhone, producer.Name, producer.Albums.Count()));
                }
                else
                {
                    sb.AppendLine(string.Format(SuccessfullyImportedProducerWithPhone, producer.Name,
                        producer.PhoneNumber, producer.Albums.Count()));
                }
            }

            context.AddRange(validProducers);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportSongs(MusicHubDbContext context, string xmlString)
            {
                var xmlSerializer = new XmlSerializer(typeof(ImportSongDto[]),
                    new XmlRootAttribute("Songs"));
                var songsDto = (ImportSongDto[])xmlSerializer.Deserialize(new StringReader(xmlString));

                var sb = new StringBuilder();
                var validSongs = new List<Song>();

                foreach (var songDto in songsDto)
                {
                    if (!IsValid(songDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    var genre = Enum.TryParse(songDto.Genre, out Genre genreResult);
                    var album = context.Albums.Find(songDto.AlbumId);
                    var writer = context.Writers.Find(songDto.WriterId);
                    var songTitle = validSongs.Any(s => s.Name == songDto.Name);

                    if (!genre || album == null || writer == null || songTitle)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    var song = AutoMapper.Mapper.Map<Song>(songDto);

                    validSongs.Add(song);
                    sb.AppendLine(string.Format(SuccessfullyImportedSong, song.Name, song.Genre, song.Duration));
                }

                context.Songs.AddRange(validSongs);
                context.SaveChanges();

                return sb.ToString().TrimEnd();
            }

        public static string ImportSongPerformers(MusicHubDbContext context, string xmlString)
        {
            var xmlSerializer = new XmlSerializer(typeof(ImportPerformerDto[]),
                new XmlRootAttribute("Performers"));
            var performerDtos = (ImportPerformerDto[])xmlSerializer.Deserialize(new StringReader(xmlString));

            var validPerformers = new List<Performer>();
            var sb = new StringBuilder();

            foreach (var performerDto in performerDtos)
            {
                if (!IsValid(performerDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var validSongsCount = context.Songs.Count(s => performerDto.PerformerSongs.Any(i => i.Id == s.Id));

                if (validSongsCount != performerDto.PerformerSongs.Length)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var performer = AutoMapper.Mapper.Map<Performer>(performerDto);

                validPerformers.Add(performer);
                sb.AppendLine(string.Format(SuccessfullyImportedPerformer, performer.FirstName,
                    performer.PerformerSongs.Count()));
            }

            context.Performers.AddRange(validPerformers);
            context.SaveChanges();

            var result = sb.ToString().TrimEnd();

            return result;
        }

        private static bool IsValid(object entity)
        {
            var validationContext = new ValidationContext(entity);
            var validationResult = new List<ValidationResult>();

            var result = Validator.TryValidateObject(entity, validationContext, validationResult, true);

            return result;
        }
    }
}