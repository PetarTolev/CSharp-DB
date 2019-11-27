namespace SoftJail.DataProcessor
{
    using AutoMapper;
    using Data;
    using Data.Models;
    using ImportDto;
    using Newtonsoft.Json;
    using Data.Models.Enums;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;


    public class Deserializer
    {
        private const string ErrorMessage = "Invalid Data";

        public static string ImportDepartmentsCells(SoftJailDbContext context, string jsonString)
        {
            var departmentsDto = JsonConvert.DeserializeObject<DepartmentImportDto[]>(jsonString)
                .ToArray();
            var validDepartments = new List<Department>();

            var sb = new StringBuilder();

            foreach (var departmentDto in departmentsDto)
            {
                var department = Mapper.Map<Department>(departmentDto);

                if (!IsValid(department) || !department.Cells.All(IsValid))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                validDepartments.Add(department);
                sb.AppendLine($"Imported {departmentDto.Name} with {departmentDto.Cells.Count()} cells");
            }

            context.Departments.AddRange(validDepartments);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportPrisonersMails(SoftJailDbContext context, string jsonString)
        {
            var prisonersDto = JsonConvert.DeserializeObject<PrisonerImportDto[]>(jsonString)
                .ToArray();
            var validPrisoners = new List<Prisoner>();

            var sb = new StringBuilder();

            foreach (var prisonerDto in prisonersDto)
            {
                var prisoner = Mapper.Map<Prisoner>(prisonerDto);

                if (!IsValid(prisoner) || !prisoner.Mails.All(IsValid))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                validPrisoners.Add(prisoner);
                sb.AppendLine($"Imported {prisoner.FullName} {prisoner.Age} years old");
            }

            context.Prisoners.AddRange(validPrisoners);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportOfficersPrisoners(SoftJailDbContext context, string xmlString)
        {
            var serializer = new XmlSerializer(typeof(OfficersImportDto[]),
                new XmlRootAttribute("Officers"));

            var officersDto = (OfficersImportDto[]) serializer.Deserialize(new StringReader(xmlString));

            var validOfficers = new List<Officer>();

            var sb = new StringBuilder();

            foreach (var officerDto in officersDto)
            {
                bool isValidPosition = Enum.IsDefined(typeof(Position), officerDto.Position);
                bool isValidWeapon = Enum.IsDefined(typeof(Weapon), officerDto.Weapon);

                if (isValidPosition == false || isValidWeapon == false)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var officer = Mapper.Map<Officer>(officerDto);
                bool isValidOfficer = IsValid(officer);

                if (isValidOfficer == false)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                officer.OfficerPrisoners = officerDto.Prisoners
                    .Select(p => new OfficerPrisoner {PrisonerId = p.Id})
                    .ToList();

                validOfficers.Add(officer);
                sb.AppendLine($"Imported {officer.FullName} ({officer.OfficerPrisoners.Count()} prisoners)");
            }

            context.Officers.AddRange(validOfficers);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object entity)
        {
            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(entity);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(entity, validationContext, validationResult, true);
        }
    }
}