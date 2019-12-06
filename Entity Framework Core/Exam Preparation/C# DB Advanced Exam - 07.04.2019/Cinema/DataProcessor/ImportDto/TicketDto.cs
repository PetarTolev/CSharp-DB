namespace Cinema.DataProcessor.ImportDto
{
    using System.Xml.Serialization;

    [XmlType("Ticket")]
    public class TicketDto
    {
        [XmlElement("ProjectionId")]
        public int ProjectionId { get; set; }
        
        [XmlElement("Price")]
        public double Price { get; set; }
    }
}