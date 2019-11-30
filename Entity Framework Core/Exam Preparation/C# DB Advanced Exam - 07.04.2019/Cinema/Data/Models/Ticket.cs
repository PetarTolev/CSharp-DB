namespace Cinema.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using static Validations.DataValidation;

    public class Ticket
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Range(MoneyMin, MoneyMax)]
        public decimal Price { get; set; }
        
        [Required]
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        
        [Required]
        public int ProjectionId { get; set; }
        public Projection Projection { get; set; }
    }
}