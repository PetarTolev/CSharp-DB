using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using TeisterMask.Data.Models.Enums;

namespace TeisterMask.DataProcessor.ImportDto
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    [XmlType("Task")]
    public class TaskDto
    {
        [XmlElement("Name")]
        [Required]
        [MinLength(2), MaxLength(40)]
        public string Name { get; set; }

        [XmlElement("OpenDate")]
        [Required]
        public string OpenDate { get; set; }
        
        [XmlElement("DueDate")]
        [Required]
        public string DueDate { get; set; }
        
        [XmlElement("ExecutionType")]
        [Required]
        public string ExecutionType { get; set; }
        
        [XmlElement("LabelType")]
        [Required]
        public string LabelType { get; set; }
    }
}