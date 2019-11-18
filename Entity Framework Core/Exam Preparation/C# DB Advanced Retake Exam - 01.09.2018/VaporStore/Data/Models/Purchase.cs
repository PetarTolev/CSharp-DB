namespace VaporStore.Data.Models
{
    using Enums;
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Purchase
    {
        public Purchase()
        {
        }

        public Purchase(Game game, PurchaseType type, Card card, string productKey, DateTime date)
        {
            this.Game = game;
            this.Type = type;
            this.Card = card;
            this.ProductKey = productKey;
            this.Date = date;
        }

        public int Id { get; set; }

        [Required]
        public PurchaseType Type { get; set; }

        [Required]
        public string ProductKey { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public int CardId { get; set; }
        public Card Card { get; set; }

        public int GameId { get; set; }
        public Game Game { get; set; }
    }
}
