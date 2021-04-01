namespace SoftJail.DataProcessor
{

    using Data;
    using Newtonsoft.Json;
    using SoftJail.DataProcessor.ExportDto;
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;

    public class Serializer
    {
        public static string ExportPrisonersByCells(SoftJailDbContext context, int[] ids)
        {
            var prisonersOutputDto = context.Prisoners
                .Where(x=>ids.Contains(x.Id))
                .Select(x => new
            {
                Id=x.Id,
                Name=x.FullName,
                CellNumber=x.Cell.CellNumber,
                Officers=x.PrisonerOfficers
                .OrderBy(x=>x.Officer.FullName)
                .Select(o=> new
                {
                    OfficerName=o.Officer.FullName,
                    Department =o.Officer.Department.Name

                })
                .ToList(),

                TotalOfficerSalary=Decimal.Round(x.PrisonerOfficers.Sum(x=>x.Officer.Salary),2)

            })
                .OrderBy(x=>x.Name)
                .ThenBy(x=>x.Id)
                .ToList();


            var json = JsonConvert.SerializeObject(prisonersOutputDto, Formatting.Indented);


                return json;

        }

        public static string ExportPrisonersInbox(SoftJailDbContext context, string prisonersNames)
        {

           
            var provider = prisonersNames.Split(",", StringSplitOptions.RemoveEmptyEntries).ToArray();

            var prisonersOutputModel = context.Prisoners
                .Where(x=>provider.Contains(x.FullName))
                .Select(x => new PrisonerOutputXmlModel
            {
                Id=x.Id,
                Name=x.FullName,
                IncarcerationDate=x.IncarcerationDate.ToString("yyyy-MM-dd",CultureInfo.InvariantCulture),
                EncryptedMessages=x.Mails.Select(x=> new MessageOutputXmlModel
                {
                    Description= new string(x.Description.Reverse().ToArray())

                }).ToArray()

            })
                .OrderBy(x=>x.Name)
                .ThenBy(x=>x.Id)
                 .ToArray();

            var serializer = new XmlSerializer(typeof(PrisonerOutputXmlModel[]), new XmlRootAttribute("Prisoners"));

            var ns = new XmlSerializerNamespaces();
            ns.Add("", "");

            var sb = new StringBuilder();

            serializer.Serialize(new StringWriter(sb), prisonersOutputModel, ns);

            return sb.ToString().TrimEnd();
            
        }
    }
}