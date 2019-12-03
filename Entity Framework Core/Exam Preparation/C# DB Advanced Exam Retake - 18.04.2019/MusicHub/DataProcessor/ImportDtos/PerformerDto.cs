namespace MusicHub.DataProcessor.ImportDtos
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;

    [XmlType("Performer")]
    public class PerformerDto
    {
        [Required]
        [MinLength(3), MaxLength(20)]
        public string FirstName { get; set; }
        
        [Required]
        [MinLength(3), MaxLength(20)]
        public string LastName { get; set; }
        
        [Required]
        [Range(18, 70)]
        public int Age { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal NetWorth { get; set; }

        [XmlArray("PerformersSongs")]
        public SongPerformerDto[] PerformerSongs { get; set; }
    }
}