namespace CarDealer.Dtos.Import
{
    using System.Xml.Serialization;

    [XmlType("Sale")]
    public class ImportSaleDto
    {
        [XmlElement("carPartIdDto")]
        public int CarId { get; set; }

        [XmlElement("customerPartIdDto")]
        public int CustomerId { get; set; }

        [XmlElement("discount")]
        public int Discount { get; set; }
    }
}