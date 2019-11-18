using System.Globalization;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using VaporStore.Data.Models.Enums;
using VaporStore.DataProcessor.Dto.Export;

namespace VaporStore.DataProcessor
{
    using Data;
    using System;
    using Newtonsoft.Json;
    using System.Linq;

    public static class Serializer
	{
        public static string ExportGamesByGenres(VaporStoreDbContext context, string[] genreNames)
        {
            var games = context.Genres
                .Where(g => genreNames.Contains(g.Name))
                .OrderByDescending(g => g.Games.Sum(game => game.Purchases.Count()))
                .Select(g => new
                {
                    Id = g.Id,
                    Genre = g.Name,
                    Games = g.Games.Select(game =>
                        new
                        {
                            Id = game.Id,
                            Title = game.Name,
                            Developer = game.Developer.Name,
                            Tags = string.Join(", ", game.GameTags.Select(gt => gt.Tag.Name)),
                            Players = game.Purchases.Count()
                        })
                        .Where(game => game.Players > 0)
                        .OrderByDescending(game => game.Players)
                        .ThenBy(game => game.Id)
                        .ToArray(),
                    TotalPlayers = g.Games.Sum(game => game.Purchases.Count())
                })
                .ToArray();

            var json = JsonConvert.SerializeObject(games, Formatting.Indented);

            return json;
        }

        public static string ExportUserPurchasesByType(VaporStoreDbContext context, string storeType)
        {
            var storeTypeValue = Enum.Parse<PurchaseType>(storeType);

            var purchases = context.Users
                .Select(user =>
                    new UserExportDto
                    {
                        Username = user.Username,
                        Purchases = user.Cards
                            .SelectMany(c => c.Purchases)
                            .Where(p => p.Type == storeTypeValue)
                            .Select(p =>
                                new PurchaseExportDto
                                {
                                    Card = p.Card.Number,
                                    Cvc = p.Card.Cvc,
                                    Date = p.Date.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture),
                                    Game =
                                        new GameExportDto
                                        {
                                            Title = p.Game.Name,
                                            Genre = p.Game.Genre.Name,
                                            Price = p.Game.Price
                                        }
                                })
                            .OrderBy(p => p.Date)
                            .ToArray(),
                        TotalSpent = user.Cards
                            .SelectMany(c => c.Purchases)
                            .Where(p => p.Type == storeTypeValue)
                            .Sum(p => p.Game.Price)
                    })
                .Where(u => u.Purchases.Any())
                .OrderByDescending(u => u.TotalSpent)
                .ThenBy(u => u.Username)
                .ToArray();

            var serializer = new XmlSerializer(typeof(UserExportDto[]),
                new XmlRootAttribute("Users"));

            var sb = new StringBuilder();
            var namespaces = new XmlSerializerNamespaces(new[] { new XmlQualifiedName("", "") });

            serializer.Serialize(new StringWriter(sb), purchases, namespaces);

            return sb.ToString().TrimEnd();
        }
    }
}