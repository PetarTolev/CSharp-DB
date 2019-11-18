namespace VaporStore.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Tag
    {
        public Tag()
        {
            this.GameTags = new HashSet<GameTag>();
        }

        public Tag(string name)
        {
            this.Name = name;
        }

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public IEnumerable<GameTag> GameTags { get; set; }
    }
}
