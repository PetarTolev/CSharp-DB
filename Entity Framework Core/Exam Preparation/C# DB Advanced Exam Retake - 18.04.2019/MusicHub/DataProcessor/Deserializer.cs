namespace MusicHub.DataProcessor
{
    using AutoMapper;
    using Data;
    using Data.Models;
    using Data.Models.Enums;
    using ImportDtos;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;

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
            var writersDto = JsonConvert.DeserializeObject<WriterDto[]>(jsonString).ToArray();
            var validWriters = new List<Writer>();

            var sb = new StringBuilder();

            foreach (var writerDto in writersDto)
            {
                var writer = Mapper.Map<Writer>(writerDto);

                if (!IsValid(writer))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                sb.AppendLine(string.Format(SuccessfullyImportedWriter, writer.Name));
                validWriters.Add(writer);
            }

            context.Writers.AddRange(validWriters);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportProducersAlbums(MusicHubDbContext context, string jsonString)
        {
            var producersDto = JsonConvert.DeserializeObject<ProducerDto[]>(jsonString).ToArray();
            var validProducers = new List<Producer>();

            var sb = new StringBuilder();

            foreach (var producerDto in producersDto)
            {
                var producer = Mapper.Map<Producer>(producerDto);

                if (!IsValid(producer) || !producer.Albums.All(IsValid))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (producer.PhoneNumber == null)
                {
                    sb.AppendLine(string.Format(SuccessfullyImportedProducerWithNoPhone, producer.Name,
                        producer.Albums.Count));
                }
                else
                {
                    sb.AppendLine(string.Format(SuccessfullyImportedProducerWithPhone, producer.Name,
                        producer.PhoneNumber, producer.Albums.Count));
                }

                validProducers.Add(producer);
            }

            context.Producers.AddRange(validProducers);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportSongs(MusicHubDbContext context, string xmlString)
        {	
            var serializer = new XmlSerializer(typeof(SongDto[]),
                new XmlRootAttribute("Songs"));

            var songsDto = (SongDto[]) serializer.Deserialize(new StringReader(xmlString));

            var validSongs = new List<Song>();

            var sb = new StringBuilder();

            foreach (var songDto in songsDto)
            {
                var validGenre = Enum.IsDefined(typeof(Genre), songDto.Genre);
                var writersId = context.Writers.Select(w => w.Id);
                var validWriter = writersId.Contains(songDto.WriterId);

                var albumsId = context.Albums.Select(a => a.Id);
                var validAlbum = albumsId.Contains(songDto.AlbumId.Value);

                if (!IsValid(songDto) || !validGenre || !validWriter || !validAlbum)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var song = new Song
                {
                    Name = songDto.Name,
                    Duration = TimeSpan.ParseExact(songDto.Duration, @"c", CultureInfo.InvariantCulture),
                    CreatedOn = DateTime.ParseExact(songDto.CreatedOn, @"dd/MM/yyyy", CultureInfo.InvariantCulture),
                    Genre = Enum.Parse<Genre>(songDto.Genre),
                    AlbumId = songDto.AlbumId,
                    WriterId = songDto.WriterId,
                    Price = songDto.Price
                };

                sb.AppendLine(string.Format(SuccessfullyImportedSong, song.Name, song.Genre.ToString(),
                    song.Duration.ToString("c")));

                validSongs.Add(song);
            }
	
            context.Songs.AddRange(validSongs);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportSongPerformers(MusicHubDbContext context, string xmlString)
        {
            var serializer = new XmlSerializer(typeof(PerformerDto[]),
                new XmlRootAttribute("Performers"));

            var performersDto = (PerformerDto[]) serializer.Deserialize(new StringReader(xmlString));

            var validPerformers = new List<Performer>();

            var sb = new StringBuilder();

            foreach (var performerDto in performersDto)
            {
                var validSongsCount = context.Songs.Count(s => performerDto.PerformerSongs.Any(i => i.Id == s.Id));
                var isValidSongs = performerDto.PerformerSongs.Length == validSongsCount;

                if (!IsValid(performerDto) || !isValidSongs)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var performer = Mapper.Map<Performer>(performerDto);

                sb.AppendLine(string.Format(SuccessfullyImportedPerformer, performer.FirstName,
                    performer.PerformerSongs.Count));

                validPerformers.Add(performer);
            }

            context.Performers.AddRange(validPerformers);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object entity)
        {
            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(entity);
            var validationResult = new List<ValidationResult>();

            var result = Validator.TryValidateObject(entity, validationContext, validationResult, true);

            return result;
        }
    }
}