namespace TeisterMask.DataProcessor.ImportDto
{
    using System.Xml.Serialization;

    [XmlType("Task")]
    public class TaskDto
    {
        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("OpenDate")]
        public string OpenDate { get; set; }
        
        [XmlElement("DueDate")]
        public string DueDate { get; set; }
        
        [XmlElement("ExecutionType")]
        public string ExecutionType { get; set; }
        
        [XmlElement("LabelType")]
        public string LabelType { get; set; }
    }
}