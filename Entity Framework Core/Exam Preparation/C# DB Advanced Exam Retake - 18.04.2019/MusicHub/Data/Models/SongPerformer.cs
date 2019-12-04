namespace MusicHub.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public class SongPerformer
    {
        [Required]
        public int SongId { get; set; }
        public Song Song { get; set; }
        
        [Required]
        public int PerformerId { get; set; }
        public Performer Performer { get; set; }
    }
}