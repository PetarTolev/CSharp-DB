namespace SoftJail.DataProcessor
{
    using Data;
    using ExportDto;
    using Newtonsoft.Json;
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;

    public class Serializer
    {
        public static string ExportPrisonersByCells(SoftJailDbContext context, int[] ids)
        {
            var prisoners = context.Prisoners
                .Where(p => ids.Contains(p.Id))
                .Select(p =>
                    new PrisonerByCellExportDto
                    {
                        Id = p.Id,
                        Name = p.FullName,
                        CellNumber = p.Cell.CellNumber,
                        Officers = p.PrisonerOfficers.Select(po =>
                                new OfficerExportDto
                                {
                                    OfficerName = po.Officer.FullName,
                                    Department = po.Officer.Department.Name
                                })
                            .OrderBy(po => po.OfficerName)
                            .ToArray(),
                        TotalOfficerSalary = Math.Round(p.PrisonerOfficers.Sum(po => po.Officer.Salary), 2)
                    })
                .OrderBy(p => p.Name)
                .ThenBy(p => p.Id)
                .ToArray();

            var serializedPrisoners = JsonConvert.SerializeObject(prisoners, Formatting.Indented);
            return serializedPrisoners;
        }

        public static string ExportPrisonersInbox(SoftJailDbContext context, string prisonersNames)
        {
            var names = prisonersNames
                .Split(',')
                .ToArray();

            var prisoners = context.Prisoners
                .Where(p => names.Contains(p.FullName))
                .Select(p =>
                    new PrisonerInboxExportDto
                    {
                        Id = p.Id,
                        Name = p.FullName,
                        IncarcerationDate = p.IncarcerationDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                        Messages = p.Mails.Select(m =>
                            new MessageExportDto
                            {
                                Description = Reverse(m.Description)
                            })
                            .ToArray()
                    })
                .OrderBy(p => p.Name)
                .ThenByDescending(p => p.Id)
                .ToArray();


            var serializer = new XmlSerializer(typeof(PrisonerInboxExportDto[]),
                new XmlRootAttribute("Prisoners"));

            var sb = new StringBuilder();
            var namespaces = new XmlSerializerNamespaces(new[] {new XmlQualifiedName("", "")});

            serializer.Serialize(new StringWriter(sb), prisoners, namespaces);

            return sb.ToString().TrimEnd();
        }

        private static string Reverse(string str)
        {
            char[] charArray = str.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }
    }
}