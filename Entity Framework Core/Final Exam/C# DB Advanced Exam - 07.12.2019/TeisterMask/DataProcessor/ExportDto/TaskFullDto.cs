namespace TeisterMask.DataProcessor.ExportDto
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class TaskFullDto
    {
        public string TaskName { get; set; }

        public string OpenDate { get; set; }

        public string DueDate { get; set; }

        public string LabelType { get; set; }

        public string ExecutionType { get; set; }
    }
}