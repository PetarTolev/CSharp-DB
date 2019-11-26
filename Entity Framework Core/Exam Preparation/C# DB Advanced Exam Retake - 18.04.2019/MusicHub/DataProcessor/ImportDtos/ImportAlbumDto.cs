namespace MusicHub.DataProcessor.ImportDtos
{
    using System.ComponentModel.DataAnnotations;

    public class ImportAlbumDto
    {
        [Required]
        [MinLength(3), MaxLength(40)]
        public string Name { get; set; }

        [Required]
        [RegularExpression(@"\d{2}\/\d{2}\/\d{4}")]
        public string ReleaseDate { get; set; }
    }
}