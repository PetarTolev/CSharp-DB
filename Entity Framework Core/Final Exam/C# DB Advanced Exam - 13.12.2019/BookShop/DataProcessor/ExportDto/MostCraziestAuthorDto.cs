namespace BookShop.DataProcessor.ExportDto
{
    public class MostCraziestAuthorDto
    {
        public string AuthorName { get; set; }

        public BookDto[] Books { get; set; }
    }

    public class BookDto
    {
        public string BookName { get; set; }

        public string BookPrice { get; set; }
    }
}
