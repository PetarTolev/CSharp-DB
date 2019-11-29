namespace Cinema.DataProcessor.ImportDto
{
    using System.Xml.Serialization;

    [XmlType("Ticket")]
    public class TicketImportDto
    {
        [XmlElement("ProjectionId")]
        public int ProjectionId { get; set; }
        
        [XmlElement("Price")]
        public decimal Price { get; set; }
    }
}