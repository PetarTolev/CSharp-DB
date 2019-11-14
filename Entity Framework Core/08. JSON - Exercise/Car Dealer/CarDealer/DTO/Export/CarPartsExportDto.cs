namespace CarDealer.DTO.Export
{
    using Models;
    using Newtonsoft.Json;

    public class CarPartsExportDto
    {
        [JsonProperty(PropertyName = "car")]
        public CarExportDto Car { get; set; }

        [JsonProperty(PropertyName = "parts")]
        public PartExportDto[] Parts { get; set; }
    }
}