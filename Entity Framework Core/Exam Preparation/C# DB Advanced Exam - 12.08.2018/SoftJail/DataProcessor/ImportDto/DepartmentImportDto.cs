namespace SoftJail.DataProcessor.ImportDto
{
    using Data.Models;
    using System.Collections.Generic;

    public class DepartmentImportDto
    {
        public string Name { get; set; }

        public ICollection<Cell> Cells { get; set; }
    }
}