namespace VaporStore.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Genre
    {
        public Genre()
        {
            this.Games = new HashSet<Game>();
        }

        public Genre(string name)
        {
            this.Name = name;
        }

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public IEnumerable<Game> Games { get; set; }
    }
}
