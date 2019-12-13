namespace BookShop.DataProcessor.ImportDto
{
    using System.Xml.Serialization;

    [XmlType("Book")]
    public class BookDto
    {
        public string Name { get; set; }

        public string Genre { get; set; }

        public string Price { get; set; }

        public string Pages { get; set; }

        public string PublishedOn { get; set; }
    }
}
