namespace TeisterMask.Data.Models
{
    using Enums;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Task
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(2), MaxLength(40)]
        public string Name { get; set; }

        [Required]
        public DateTime OpenDate { get; set; }
        
        [Required]
        public DateTime DueDate { get; set; }
        
        [Required]
        public ExecutionType ExecutionType { get; set; }
        
        [Required]
        public LabelType LabelType { get; set; }
        
        [Required]
        public int ProjectId { get; set; }
        public Project Project { get; set; }

        public ICollection<EmployeeTask> EmployeesTasks { get; set; } = new HashSet<EmployeeTask>();
    }
}