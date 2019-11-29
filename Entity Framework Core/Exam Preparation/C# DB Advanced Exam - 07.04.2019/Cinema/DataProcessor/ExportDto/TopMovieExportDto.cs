namespace Cinema.DataProcessor.ExportDto
{
    public class TopMovieExportDto
    {
        public string MovieName { get; set; }

        public string Rating { get; set; }

        public string TotalIncomes { get; set; }

        public CustomerExportDto[] Customers { get; set; }
    }
}