namespace SoftJail.DataProcessor.ExportDto
{
    public class PrisonerByCellExportDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int CellNumber { get; set; }

        public OfficerExportDto[] Officers { get; set; }

        public decimal TotalOfficerSalary { get; set; }
    }
}