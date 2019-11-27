namespace SoftJail.DataProcessor.ImportDto
{
    using System.Xml.Serialization;

    [XmlType("Officer")]
    public class OfficersImportDto
    {
        [XmlElement("Name")]
        public string FullName { get; set; }

        [XmlElement("Money")]
        public decimal Salary { get; set; }

        [XmlElement("Position")]
        public string Position { get; set; }

        [XmlElement("Weapon")]
        public string Weapon { get; set; }

        [XmlElement("DepartmentId")]
        public int DepartmentId { get; set; }

        [XmlArray("Prisoners")]
        public PrisonerIdDto[] Prisoners { get; set; }
    }
}