namespace VaporStore.DataProcessor
{
    using Data;
    using Data.Models;
    using Dto.Import;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;

    public static class Deserializer
	{
		public static string ImportGames(VaporStoreDbContext context, string jsonString)
        {
            var gameDtos = JsonConvert.DeserializeObject<GameImportDto[]>(jsonString)
                .ToArray();

            var sb = new StringBuilder();

            var games = new List<Game>();
            var developers = new List<Developer>();
            var genres = new List<Genre>();
            var tags = new List<Tag>();

            foreach (var gameDto in gameDtos)
            {
                if (!IsValid(gameDto) || !gameDto.Tags.All(IsValid))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                var developer = developers.FirstOrDefault(d => d.Name == gameDto.Developer);
                if (developer == null)
                {
                    developer = new Developer(gameDto.Developer);
                    developers.Add(developer);
                }

                var genre = genres.SingleOrDefault(g => g.Name == gameDto.Genre);
                if (genre == null)
                {
                    genre = new Genre(gameDto.Genre);
                    genres.Add(genre);
                }

                var gameTags = new List<Tag>();
                foreach (var tagName in gameDto.Tags)
                {
                    var tag = tags.SingleOrDefault(t => t.Name == tagName);
                    if (tag == null)
                    {
                        tag = new Tag(tagName);
                        tags.Add(tag);
                    }

                    gameTags.Add(tag);
                }

                var game = new Game(
                    gameDto.Name,
                    gameDto.Price,
                    gameDto.ReleaseDate,
                    developer,
                    genre,
                    gameTags.Select(t => new GameTag{Tag = t}).ToArray()
                );

                games.Add(game);
                sb.AppendLine($"Added {game.Name} ({game.Genre.Name}) with {game.GameTags.Count()} tags");
            }

            context.Games.AddRange(games);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

		public static string ImportUsers(VaporStoreDbContext context, string jsonString)
        {
            var userDtos = JsonConvert.DeserializeObject<UserImportDto[]>(jsonString)
                .ToArray();

            var users = new List<User>();

            var sb = new StringBuilder();

            foreach (var userDto in userDtos)
            {
                if (!IsValid(userDto) || !userDto.Cards.All(IsValid))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                var cards = userDto.Cards
                    .Select(c => new Card(c.Number, c.Cvc, c.Type))
                    .ToArray();

                users.Add(new User(
                    userDto.FullName,
                    userDto.Username,
                    userDto.Email,
                    userDto.Age,
                    cards));

                sb.AppendLine($"Imported {userDto.Username} with {userDto.Cards.Length} cards");
            }

            context.AddRange(users);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

		public static string ImportPurchases(VaporStoreDbContext context, string xmlString)
		{
            var serializer = new XmlSerializer(typeof(PurchaseImportDto[]),
                new XmlRootAttribute("Purchases"));

            var purchaseDtos = (PurchaseImportDto[]) serializer.Deserialize(new StringReader(xmlString));
            
            var purchases = new List<Purchase>();

            var sb = new StringBuilder();

            foreach (var purchaseDto in purchaseDtos)
            {
                if (!IsValid(purchaseDto))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                var game = context.Games.FirstOrDefault(g => g.Name == purchaseDto.Title);
                var card = context.Cards.Include(c => c.User).FirstOrDefault(c => c.Number == purchaseDto.Card);
                var date = DateTime.ParseExact(purchaseDto.Date, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);

                var purchase = new Purchase(
                    game,
                    purchaseDto.Type,
                    card,
                    purchaseDto.Key,
                    date);

                purchases.Add(purchase);
                sb.AppendLine($"Imported {purchase.Game.Name} for {purchase.Card.User.Username}");
            }

            context.Purchases.AddRange(purchases);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(this object obj)
        {
            var validationContext = new ValidationContext(obj);
            var validationResults = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(obj, validationContext, validationResults, true);

            return isValid;
        }
    }
}