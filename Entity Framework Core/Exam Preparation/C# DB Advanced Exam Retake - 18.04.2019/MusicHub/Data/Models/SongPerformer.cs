namespace MusicHub.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public class SongPerformer
    {
        [Required]
        public int SongId { get; set; } //todo: not sure for required
        public Song Song { get; set; }
        
        [Required]
        public int PerformerId { get; set; }
        public Performer Performer { get; set; }
    }
}