namespace FastFood.DataProcessor.Dto.Export
{
    public class EmployeeExportDto
    {
        public string Name { get; set; }

        public OrderExportDto[] Orders { get; set; }

        public decimal TotalMade { get; set; }
    }
}