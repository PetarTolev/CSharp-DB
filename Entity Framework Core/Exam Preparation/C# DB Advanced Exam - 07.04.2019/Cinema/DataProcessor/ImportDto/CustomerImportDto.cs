﻿namespace Cinema.DataProcessor.ImportDto
{
    using System.Xml.Serialization;

    [XmlType("Customer")]
    public class CustomerImportDto
    {
        [XmlElement("FirstName")]
        public string FirstName { get; set; }
        
        [XmlElement("LastName")]
        public string LastName { get; set; }
        
        [XmlElement("Age")]
        public int Age { get; set; }
        
        [XmlElement("Balance")]
        public decimal Balance { get; set; }

        [XmlArray("Tickets")]
        public TicketImportDto[] Tickets { get; set; }
    }
}