namespace FastFood.DataProcessor.Dto.Import
{
    using System.Xml.Serialization;

    [XmlType("Order")]
    public class OrderImportDto
    {
        [XmlElement("Customer")]
        public string CustomerName { get; set; }
        
        [XmlElement("Employee")]
        public string EmployeeName { get; set; }
        
        [XmlElement("DateTime")]
        public string DateTime { get; set; }

        [XmlElement("Type")]
        public string Type { get; set; }

        [XmlArray("Items")]
        public ItemForOrderImportDto[] Items { get; set; }
    }
}