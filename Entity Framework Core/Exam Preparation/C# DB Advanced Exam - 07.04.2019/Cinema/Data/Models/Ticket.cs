namespace Cinema.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Ticket
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }
        
        [Required]
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        
        [Required]
        public int ProjectionId { get; set; }
        public Projection Projection { get; set; }
    }
}