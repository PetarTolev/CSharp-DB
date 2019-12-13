namespace TeisterMask.DataProcessor.ImportDto
{
    using System.Xml.Serialization;

    [XmlType("Project")]
    public class ProjectDto
    {
        [XmlElement("Name")]
        public string Name { get; set; }
        
        [XmlElement("OpenDate")]
        public string OpenDate { get; set; }
        
        [XmlElement("DueDate")]
        public string DueDate { get; set; }

        [XmlArray("Tasks")]
        public TaskDto[] Tasks { get; set; }
    }
}