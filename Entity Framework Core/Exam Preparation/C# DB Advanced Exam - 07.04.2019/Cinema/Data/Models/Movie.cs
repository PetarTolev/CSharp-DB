namespace Cinema.Data.Models
{
    using Enums;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using static Validations.DataValidation;
    using static Validations.DataValidation.Movie;

    public class Movie
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(TitleMinLength), MaxLength(TitleMaxLength)]
        public string Title { get; set; }
        
        [Required]
        public Genre Genre { get; set; }
        
        [Required]
        public TimeSpan Duration { get; set; }
        
        [Required]
        [Range(RatingMin, RatingMax)]
        public double Rating { get; set; }
        
        [Required]
        [MinLength(NameMinLength), MaxLength(NameMaxLength)]
        public string Director { get; set; }

        public ICollection<Projection> Projections { get; set; } = new HashSet<Projection>();
    }
}
