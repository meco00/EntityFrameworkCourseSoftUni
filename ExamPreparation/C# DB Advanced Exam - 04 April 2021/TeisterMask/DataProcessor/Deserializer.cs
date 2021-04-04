namespace TeisterMask.DataProcessor
{
    using System;
    using System.Collections.Generic;

    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Data;
    using Newtonsoft.Json;
    using TeisterMask.Data.Models;
    using TeisterMask.Data.Models.Enums;
    using TeisterMask.DataProcessor.ImportDto;
    using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedProject
            = "Successfully imported project - {0} with {1} tasks.";

        private const string SuccessfullyImportedEmployee
            = "Successfully imported employee - {0} with {1} tasks.";

        public static string ImportProjects(TeisterMaskContext context, string xmlString)
        {
            var sb = new StringBuilder();

            var serializer =
                new XmlSerializer(typeof(ImportProjectXmlModel[]),
                new XmlRootAttribute("Projects"));

            var reader = new StringReader(xmlString);

            var projectsWithTasksInputDto = serializer.Deserialize(reader) as ImportProjectXmlModel[];

            ;

            foreach (var projectDto in projectsWithTasksInputDto)
            {
                if (!IsValid(projectDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;

                }

                var isOpenDateValid = DateTime
                    .TryParseExact
                    (projectDto.OpenDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var openDate);

                if (!isOpenDateValid)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var isDueDateValid=
                DateTime
                    .TryParseExact
                    (projectDto.DueDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var dueDate);



                var currentProject = new Project
                {
                    Name = projectDto.Name,
                    OpenDate = openDate,
                    DueDate = isDueDateValid ? (DateTime?)dueDate : null

                };

                foreach (var taskDto in projectDto.Tasks)
                {

                    if (!IsValid(taskDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    var isOpenTaskDateValid = DateTime
                   .TryParseExact
                   (taskDto.OpenDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var openDateTask);


                    var isDueTaskDateValid = DateTime
                   .TryParseExact
                   (taskDto.DueDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var dueDateTask);

                    if (!isOpenDateValid || !isDueTaskDateValid)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                        var currentTask = new Task
                    {
                        Name = taskDto.Name,
                        OpenDate = openDateTask,
                        DueDate = dueDateTask,
                        ExecutionType = Enum.Parse<ExecutionType>(taskDto.ExecutionType),
                        LabelType = Enum.Parse<LabelType>(taskDto.LabelType),
                    };



                    if (DateTime.Compare(currentTask.OpenDate,currentProject.OpenDate)<0)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    if (currentProject.DueDate.HasValue && DateTime.Compare(currentProject.DueDate.Value , currentTask.DueDate)<0)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    currentProject.Tasks.Add(currentTask);


                }

                context.Projects.Add(currentProject);

                context.SaveChanges();


                sb.AppendLine(string.Format(SuccessfullyImportedProject, currentProject.Name, currentProject.Tasks.Count()));

            }

            return sb.ToString().TrimEnd();

        }

        public static string ImportEmployees(TeisterMaskContext context, string jsonString)
        {
            var sb = new StringBuilder();

            var employeesInputModel = JsonConvert.DeserializeObject<IEnumerable<ImportEmployeesJson>>(jsonString);

            ;


            foreach (var employeeDto in employeesInputModel)
            {

                if (!IsValid(employeeDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var currentEmployee = new Employee
                {
                    Username = employeeDto.Username,
                    Email = employeeDto.Email,
                    Phone = employeeDto.Phone

                };

                foreach (var tasksId in employeeDto.Tasks.Distinct())
                {

                  var task= context.Tasks.FirstOrDefault(x => x.Id == tasksId);

                    if (task==null)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;

                    }

                    currentEmployee.EmployeesTasks.Add(new EmployeeTask
                    {
                        Employee = currentEmployee,
                        Task = task
                    });




                }

                context.Employees.Add(currentEmployee);

                ;

                context.SaveChanges();

                sb
                 .AppendLine(string.Format(SuccessfullyImportedEmployee, currentEmployee.Username, currentEmployee.EmployeesTasks.Count()));


            }

            return sb.ToString().Trim();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}