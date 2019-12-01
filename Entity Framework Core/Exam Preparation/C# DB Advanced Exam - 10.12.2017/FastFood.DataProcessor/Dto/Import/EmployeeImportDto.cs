namespace FastFood.DataProcessor.Dto.Import
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;

    public class EmployeeImportDto
    {
        [Required]
        [MinLength(3), MaxLength(30)]
        public string Name { get; set; }
        
        [Required]
        [Range(15, 80)]
        public int Age { get; set; }

        [JsonProperty("Position")]
        [Required]
        [MinLength(3), MaxLength(30)]
        public string PositionName { get; set; }
    }
}