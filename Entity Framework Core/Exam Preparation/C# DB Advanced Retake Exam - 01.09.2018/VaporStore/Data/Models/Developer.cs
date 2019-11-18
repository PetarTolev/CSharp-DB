namespace VaporStore.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Developer
    {
        public Developer()
        {
            this.Games = new HashSet<Game>();
        }

        public Developer(string name)
        {
            this.Name = name;
        }

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public IEnumerable<Game> Games { get; set; }
    }
}
