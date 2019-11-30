namespace Cinema.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Validations;
    using static Validations.DataValidation;
    using static Validations.DataValidation.Customer;

    public class Customer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(DataValidation.Customer.NameMinLength), MaxLength(DataValidation.Customer.NameMaxLength)]
        public string FirstName { get; set; }
        
        [Required]
        [MinLength(DataValidation.Customer.NameMinLength), MaxLength(DataValidation.Customer.NameMaxLength)]
        public string LastName { get; set; }
        
        [Required]
        [Range(AgeMin, AgeMax)]
        public int Age { get; set; }
        
        [Required]
        [Range(MoneyMin, MoneyMax)]
        public decimal Balance { get; set; }

        public ICollection<Ticket> Tickets { get; set; } = new HashSet<Ticket>();
    }
}