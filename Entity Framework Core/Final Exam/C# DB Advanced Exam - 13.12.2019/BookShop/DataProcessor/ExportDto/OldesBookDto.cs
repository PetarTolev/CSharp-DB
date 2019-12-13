namespace BookShop.DataProcessor.ExportDto
{
    using System.Xml.Serialization;

    [XmlType("Book")]
    public class OldesBookDto
    {
        [XmlAttribute("Pages")]
        public string Pages { get; set; }

        public string Name { get; set; }

        public string Date { get; set; }
    }
}
