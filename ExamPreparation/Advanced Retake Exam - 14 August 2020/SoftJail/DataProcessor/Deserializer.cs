namespace SoftJail.DataProcessor
{
    using Data;
    using Newtonsoft.Json;
    using SoftJail.Data.Models;
    using SoftJail.Data.Models.Enums;
    using SoftJail.DataProcessor.ImportDto;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;

    public class Deserializer
    {
        private const string ERROR_MESSAGE = "Invalid Data";

        public static string ImportDepartmentsCells(SoftJailDbContext context, string jsonString)
        {
            var sb = new StringBuilder();

            var departmentCellsInputModel = JsonConvert.DeserializeObject<IEnumerable<DepartmentJsonInputModel>>(jsonString);

            
            

            foreach (var departmentDto in departmentCellsInputModel)
            {
                if (!IsValid(departmentDto) || !departmentDto.Cells.All(IsValid)|| !departmentDto.Cells.Any())
                {
                    sb.AppendLine(ERROR_MESSAGE);
                    continue;

                }

                var currentDepartment = new Department
                {
                    Name = departmentDto.Name,
                };

                foreach (var cell in departmentDto.Cells)
                {
                    currentDepartment.Cells.Add(new Cell
                    {
                        CellNumber=cell.CellNumber.Value,
                        HasWindow=cell.HasWindow.Value,
                        Department=currentDepartment

                    });

                }

                ;

                context.Departments.Add(currentDepartment);
                context.SaveChanges();

                sb.AppendLine($"Imported {currentDepartment.Name} with {currentDepartment.Cells.Count()} cells");




            }

            

            return sb.ToString().TrimEnd();
        }

        public static string ImportPrisonersMails(SoftJailDbContext context, string jsonString)
        {
            var sb = new StringBuilder();

            var prisonersInputModel = 
                JsonConvert
                .DeserializeObject<IEnumerable<ImportPrisonersAndMailsJsonInputModel>>(jsonString);


            ;

            foreach (var prisonerDTO in prisonersInputModel)
            {
                if (!IsValid(prisonerDTO)|| !prisonerDTO.Mails.All(IsValid))
                {
                    sb.AppendLine(ERROR_MESSAGE);
                    continue;

                }

                ;

                bool isIncarcerationDateValid = 
                    DateTime
                    .TryParseExact(prisonerDTO.IncarcerationDate, "dd/MM/yyyy",CultureInfo.InvariantCulture, DateTimeStyles.None, out var increcationDate);

                if (!isIncarcerationDateValid)
                {
                    sb.AppendLine(ERROR_MESSAGE);
                    continue;
                }


              



                var currentPrisoner = new Prisoner
                {
                    FullName = prisonerDTO.FullName,
                    Nickname = prisonerDTO.Nickname,
                    Age = prisonerDTO.Age.Value,
                    IncarcerationDate = increcationDate,
                    ReleaseDate = DateTime.TryParse(prisonerDTO.ReleaseDate, out var date) ? date : (DateTime?)null,
                    Bail =  prisonerDTO.Bail.HasValue  ? prisonerDTO.Bail : null,
                    CellId = prisonerDTO.CellId.HasValue ? prisonerDTO.CellId : null,
                    Mails = prisonerDTO.Mails.Select(x => new Mail
                    {
                        Description=x.Description,
                        Sender=x.Sender,
                        Address=x.Address

                    }).ToList()
                };


                context.Prisoners.Add(currentPrisoner);

                context.SaveChanges();

                sb.AppendLine($"Imported {currentPrisoner.FullName} {currentPrisoner.Age} years old");

                

            }

;
            return sb.ToString().TrimEnd();
        }

        public static string ImportOfficersPrisoners(SoftJailDbContext context, string xmlString)
        {
            var sb = new StringBuilder();

            var serializer = 
                new XmlSerializer(typeof(ImportOfficerPrisonersXmlInputModel[]),
                new XmlRootAttribute("Officers"));

            var reader = new StringReader(xmlString);

            var officerPrisonersInputModel = serializer.Deserialize(reader) as ImportOfficerPrisonersXmlInputModel[];

            foreach (var officerPrisonerDto in officerPrisonersInputModel)
            {
                if (!IsValid(officerPrisonerDto))
                {
                    sb.AppendLine(ERROR_MESSAGE);

                    continue;

                }


                var currentOfficer = new Officer
                {
                    FullName = officerPrisonerDto.FullName,
                    Salary = officerPrisonerDto.Salary,
                    Position = Enum.Parse<Position>(officerPrisonerDto.Position),
                    Weapon = Enum.Parse<Weapon>(officerPrisonerDto.Weapon),
                    DepartmentId = officerPrisonerDto.DepartmentId.Value
                };

                foreach (var prisonerId in officerPrisonerDto.Prisoners)
                {
                    currentOfficer.OfficerPrisoners.Add(new OfficerPrisoner
                    {
                        Officer = currentOfficer,
                        PrisonerId = prisonerId.Id
                    });



                }

                ;

                context.Officers.Add(currentOfficer);

                context.SaveChanges();

                sb.AppendLine($"Imported {currentOfficer.FullName } ({currentOfficer.OfficerPrisoners.Count()} prisoners)");




            }

            return sb.ToString().TrimEnd();

        }

        private static bool IsValid(object obj)
        {
            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(obj);
            var validationResult = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(obj, validationContext, validationResult, true);
            return isValid;
        }
    }
}