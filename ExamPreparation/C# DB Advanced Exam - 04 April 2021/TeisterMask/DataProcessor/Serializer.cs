namespace TeisterMask.DataProcessor
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Data;
    using Newtonsoft.Json;
    using TeisterMask.DataProcessor.ExportDto;
    using Formatting = Newtonsoft.Json.Formatting;

    public class Serializer
    {
        public static string ExportProjectWithTheirTasks(TeisterMaskContext context)
        {

            var projectsWithTheirTasks = context.Projects
                .ToArray()
                .Where(x=>x.Tasks.Any())
                .Select(x => new ProjectExportXmlModel
                {
                    TasksCount=x.Tasks.Count(),
                    ProjectName=x.Name,
                    HasEndDate=x.DueDate.HasValue ? "Yes":"No",
                    Tasks=x.Tasks.Select(t=> new TaskExportXmlModel
                    {
                        Name=t.Name,
                        Label=t.LabelType.ToString()
                    })
                    .OrderBy(t=>t.Name)
                    .ToArray()



                })
                .OrderByDescending(x=>x.TasksCount)
                .ThenBy(x=>x.ProjectName)
                .ToArray();



            var serializer = new XmlSerializer(typeof(ProjectExportXmlModel[]), new XmlRootAttribute("Projects"));

            var ns = new XmlSerializerNamespaces();
            ns.Add("", "");

            var sb = new StringBuilder();

            serializer.Serialize(new StringWriter(sb), projectsWithTheirTasks, ns);




            return sb.ToString().Trim();
        }

        public static string ExportMostBusiestEmployees(TeisterMaskContext context, DateTime date)
        {
            var mostBusiestEmployees = context.Employees.ToArray()
                .Where(x=>x.EmployeesTasks.Any(t=>t.Task.OpenDate >=date))
                .Select(x => new
            {
                Username=x.Username,
                Tasks=x.EmployeesTasks
                .Where(t => t.Task.OpenDate >= date)
                 .OrderByDescending(t => t.Task.DueDate)
                .ThenBy(t => t.Task.Name)
                .Select(t=> new
                {
                    TaskName=t.Task.Name,
                    OpenDate=t.Task.OpenDate.ToString("d",CultureInfo.InvariantCulture),
                    DueDate=t.Task.DueDate.ToString("d", CultureInfo.InvariantCulture),
                    LabelType=t.Task.LabelType.ToString(),
                    ExecutionType = t.Task.ExecutionType.ToString()


                })
               
                .ToArray()

            })
                .OrderByDescending(x=>x.Tasks.Count())
                .ThenBy(x=>x.Username)
                .Take(10)
                .ToArray();

            var json = JsonConvert.SerializeObject(mostBusiestEmployees, Formatting.Indented);


            return json;


        }
        }
}