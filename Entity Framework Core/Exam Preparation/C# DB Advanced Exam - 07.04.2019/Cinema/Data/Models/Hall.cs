namespace Cinema.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using static Validations.DataValidation;

    public class Hall
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(NameMinLength), MaxLength(NameMaxLength)]
        public string Name { get; set; }

        public bool Is4Dx { get; set; }

        public bool Is3D { get; set; }

        public ICollection<Projection> Projections { get; set; } = new HashSet<Projection>();

        public List<Seat> Seats { get; set; } = new List<Seat>();
    }
}