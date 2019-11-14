namespace CarDealer.DTO.Import
{
    using System.Collections.Generic;

    public class CarImportDto
    {
        public string Make { get; set; }

        public string Model { get; set; }

        public int TravelledDistance { get; set; }

        public List<int> PartsId { get; set; }
    }
}