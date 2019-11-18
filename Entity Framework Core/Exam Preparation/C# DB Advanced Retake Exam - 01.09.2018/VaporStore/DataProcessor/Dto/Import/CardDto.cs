namespace VaporStore.DataProcessor.Dto.Import
{
    using System.ComponentModel.DataAnnotations;
    using Data.Models.Enums;

    public class CardDto
    {
        [Required]
        [RegularExpression(@"\d{4} \d{4} \d{4} \d{4}")]
        public string Number { get; set; }

        [StringLength(3, MinimumLength = 3)]
        public string Cvc { get; set; }

        [Required]
        public CardType Type { get; set; }
    }
}