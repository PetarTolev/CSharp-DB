namespace FastFood.DataProcessor.Dto.Import
{
    using System.Xml.Serialization;

    [XmlType("Item")]
    public class ItemForOrderImportDto
    {
        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("Quantity")]
        public int Quantity { get; set; }
    }
}