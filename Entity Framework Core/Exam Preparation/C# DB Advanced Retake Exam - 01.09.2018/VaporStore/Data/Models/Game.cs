using System.ComponentModel.DataAnnotations;

namespace VaporStore.Data.Models
{
    using System;
    using System.Collections.Generic;

    public class Game
    {
        public Game()
        {
            this.Purchases = new HashSet<Purchase>();
            this.GameTags = new HashSet<GameTag>();
        }

        public Game(string name, decimal price, DateTime releaseDate, Developer developer, Genre genre, ICollection<GameTag> gameTags)
        {
            this.Name = name;
            this.Price = price;
            this.ReleaseDate = releaseDate;
            this.Developer = developer;
            this.Genre = genre;
            this.GameTags = gameTags;
        }

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [Range(0.0, Double.MaxValue)]
        public decimal Price { get; set; }

        [Required]
        public DateTime ReleaseDate { get; set; }

        [Required]
        public int DeveloperId { get; set; }
        [Required]
        public Developer Developer { get; set; }

        [Required]
        public int GenreId { get; set; }
        [Required]
        public Genre Genre { get; set; }

        public IEnumerable<Purchase> Purchases { get; set; }

        public IEnumerable<GameTag> GameTags { get; set; }
    }
}
