namespace Cinema.DataProcessor.ImportDto
{
    using System.Xml.Serialization;

    [XmlType("Customer")]
    public class CustomerDto
    {
        [XmlElement("FirstName")]
        public string FirstName { get; set; }
        
        [XmlElement("LastName")]
        public string LastName { get; set; }
        
        [XmlElement("Age")]
        public string Age { get; set; }
        
        [XmlElement("Balance")]
        public double Balance { get; set; }

        [XmlArray("Tickets")]
        public TicketDto[] Tickets { get; set; }
    }
}