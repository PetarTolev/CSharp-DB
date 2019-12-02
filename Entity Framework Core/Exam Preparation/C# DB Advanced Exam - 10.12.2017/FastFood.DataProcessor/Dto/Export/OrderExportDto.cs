namespace FastFood.DataProcessor.Dto.Export
{
    public class OrderExportDto
    {
        public string Customer { get; set; }

        public ItemExportDto[] Items { get; set; }

        public decimal TotalPrice { get; set; }
    }
}