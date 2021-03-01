
using Microsoft.EntityFrameworkCore;
using SoftUni.Data;
using SoftUni.Models;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace SoftUni
{
    public class StartUp
    {
        static void Main(string[] args)
        {


            var context = new SoftUniContext();
            using (context)
            {


                ////P3
                //Console.WriteLine( GetEmployeesFullInformation(context));

                //P4
                //Console.WriteLine(GetEmployeesWithSalaryOver50000(context));

                //P5
                //Console.WriteLine(GetEmployeesFromResearchAndDevelopment(context));

                //P6
                //Console.WriteLine(AddNewAddressToEmployee(context));

                //P7
                //Console.WriteLine(GetEmployeesInPeriod(context));

                //P8
                //Console.WriteLine(GetAddressesByTown(context));

                //P9
                //Console.WriteLine(GetEmployee147(context));

                //P10
                //Console.WriteLine(GetDepartmentsWithMoreThan5Employees(context));

                //P11
                //Console.WriteLine(GetLatestProjects(context));

                ////P12
                //Console.WriteLine(IncreaseSalaries(context));

                //P13
                //Console.WriteLine(GetEmployeesByFirstNameStartingWithSa(context));

                //P14
                //Console.WriteLine(DeleteProjectById(context));

                
                Console.WriteLine(RemoveTown(context));
            }

        }

        public static string GetEmployeesFullInformation(SoftUniContext context)
        {

            var employees = context.Employees
                .OrderBy(x => x.EmployeeId)
                .ToList();


            var sb = new StringBuilder();

            foreach (var employee in employees)
            {
                //Guy Gilbert R Production Technician 12500.00

                sb.AppendLine($"{employee.FirstName} {employee.MiddleName} {employee.LastName} {employee.JobTitle} {employee.Salary:f2} ");
            }


            return sb.ToString().Trim();
        }

        public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
        {
            var employees = context.Employees.Where(x => x.Salary > 50000)
               .OrderBy(x => x.FirstName)
               .Select(x => new
               {
                   FirstName = x.FirstName,
                   Salary = x.Salary
               })
               .ToList();


            var sb = new StringBuilder();

            foreach (var employee in employees)
            {
                sb.AppendLine($"{employee.FirstName} - {employee.Salary:f2}");
            }

            return sb.ToString().Trim();
        }

        public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
        {
            var employees = context.Employees.Where(x => x.Department.Name == "Research and Development")
                .OrderBy(x => x.Salary)
                .ThenByDescending(x => x.FirstName)
                .Select(x => new
                {
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    DepartmentName = x.Department.Name,
                    Salary = x.Salary
                }
                ).ToList();

            var sb = new StringBuilder();

            foreach (var employee in employees)
            {
                //Gigi Matthew from Research and Development - $40900.00
                sb.AppendLine($"{employee.FirstName} {employee.LastName} from {employee.DepartmentName} - ${employee.Salary:f2}");
            }

            return sb.ToString().Trim();

        }

        public static string AddNewAddressToEmployee(SoftUniContext context)
        {
            var nakov = context.Employees.FirstOrDefault(x => x.LastName == "Nakov");

            nakov.Address = new Address { AddressText = "Vitoshka 15", TownId = 4 };

            context.SaveChanges();

            var employees = context.Employees
                .OrderByDescending(x => x.AddressId)
                .Take(10)
                .Select(x => new
                {
                    AddressText = x.Address.AddressText
                })
                .ToList();

            var sb = new StringBuilder();

            foreach (var item in employees)
            {
                sb.AppendLine(item.AddressText);
            }


            return sb.ToString().Trim();


        }


        public static string GetEmployeesInPeriod(SoftUniContext context)
        {
            var employees = context.Employees
                .Include(x => x.Manager)
                .Include(x => x.EmployeesProjects)
                .ThenInclude(x => x.Project)
                .Where(x => x.EmployeesProjects.Any(p => p.Project.StartDate.Year >= 2001 && p.Project.StartDate.Year <= 2003))
                .Select(x => new
                {
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    ManagerFirstName = x.Manager.FirstName,
                    ManagerLastName = x.Manager.LastName,
                    Projects = x.EmployeesProjects.Select(p => new
                    {
                        Name = p.Project.Name,
                        StartDate = p.Project.StartDate.ToString("MM/dd/yyyy hh:mm tt", CultureInfo.InvariantCulture),
                        EndDate = p.Project.EndDate.HasValue ?
                          p.Project.EndDate.Value.ToString("MM/dd/yyyy hh:mm tt", CultureInfo.InvariantCulture)
                          : "not finished"
                    })
                })
                .Take(10)
               .ToList();


            var sb = new StringBuilder();

            foreach (var employee in employees)
            {
                sb.AppendLine($"{employee.FirstName} {employee.LastName} - " +
                    $"Manager: {employee.ManagerFirstName} {employee.ManagerLastName}");

                foreach (var project in employee.Projects)
                {


                    sb.AppendLine($"--{project.Name} - {project.StartDate} - {project.EndDate}");


                }
            }

            return sb.ToString().Trim();
        }

        public static string GetAddressesByTown(SoftUniContext context)
        {
            var addresses = context.Addresses
                .OrderByDescending(x => x.Employees.Count())
                .ThenBy(x => x.Town.Name)
                .ThenBy(x => x.AddressText)
                .Take(10)
                .Select(x => new
                {
                    AddressText = x.AddressText,
                    TownName = x.Town.Name,
                    EmployeeCount = $"{x.Employees.Count()} employees"
                })
                .ToList();

            var sb = new StringBuilder();

            foreach (var address in addresses)
            {

                sb.AppendLine($"{address.AddressText}, {address.TownName} - {address.EmployeeCount}");

            }



            return sb.ToString().Trim();
        }

        public static string GetEmployee147(SoftUniContext context)
        {
            var employee = context.Employees
               .Where(x => x.EmployeeId == 147)
                .Select(x => new
                {
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    JobTitle = x.JobTitle,
                    Projects = x.EmployeesProjects.Select(x => new
                    {
                        Name = x.Project.Name
                    })
                    .OrderBy(x => x.Name)
                    .ToList()


                })
                .SingleOrDefault();

            var sb = new StringBuilder();
            sb.AppendLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle}");

            foreach (var project in employee.Projects)
            {
                sb.AppendLine(project.Name);

            }

            return sb.ToString().Trim();
        }

        public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext context)
        {


            var departments = context.Departments
                .Where(x => x.Employees.Count() > 5)
                .OrderBy(x => x.Employees.Count)
                .ThenBy(x => x.Name)
                .Select(x => new
                {
                    DepartmentName = x.Name,
                    ManagerFirstName = x.Manager.FirstName,
                    ManagerLastName = x.Manager.LastName,
                    Employees = x.Employees.Select(x => new
                    {
                        FirstName = x.FirstName,
                        LastName = x.LastName,
                        JobTitle = x.JobTitle
                    })
                    .OrderBy(x => x.FirstName)
                    .ThenBy(x => x.LastName)
                    .ToList()

                });

            var sb = new StringBuilder();

            foreach (var department in departments)
            {
                sb.AppendLine($"{department.DepartmentName} - {department.ManagerFirstName} {department.ManagerLastName}");

                foreach (var employees in department.Employees)
                {

                    sb.AppendLine($"{employees.FirstName} {employees.LastName} - {employees.JobTitle}");

                }
            }

            return sb.ToString().Trim();

        }

        public static string GetLatestProjects(SoftUniContext context)
        {
            var projects = context.Projects
                .OrderByDescending(x => x.StartDate)
                .Take(10)
                .OrderBy(x => x.Name)
                .ToList();

            var sb = new StringBuilder();

            foreach (var project in projects)
            {
                sb.AppendLine($"{project.Name}" + Environment.NewLine +
                   $"{project.Description}" + Environment.NewLine +
                   $"{project.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture)}");
            }

            return sb.ToString().Trim();
        }

        public static string IncreaseSalaries(SoftUniContext context)
        {
            var departmentNames = new string[]
            {
                "Engineering",
                "Tool Design",
                "Marketing" ,
                "Information Services",
            };


            var employeesToUpdate = context.Employees
                .Where(x => departmentNames.Contains(x.Department.Name))

                .OrderBy(x => x.FirstName)
                .ThenBy(x => x.LastName)
                .ToList();

            var sb = new StringBuilder();



            foreach (var employee in employeesToUpdate)
            {
                employee.Salary *= 1.12M;
                sb.AppendLine($"{employee.FirstName} {employee.LastName} (${employee.Salary:f2})");
            }

            context.SaveChanges();
            return sb.ToString().Trim();

        }

        public static string GetEmployeesByFirstNameStartingWithSa(SoftUniContext context)
        {
            var employees = context.Employees
                // .Where(x => x.FirstName.StartsWith("Sa"))
                .Where(x => EF.Functions.Like(x.FirstName, "sa%"))
                .Select(x => new
                {
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    JobTitle = x.JobTitle,
                    Salary = $"{x.Salary:f2}",
                })
                .OrderBy(x => x.FirstName)
                .ThenBy(x => x.LastName)
                .ToList();

            var sb = new StringBuilder();

            foreach (var emp in employees)
            {
                sb.AppendLine($"{emp.FirstName} {emp.LastName} - {emp.JobTitle} - (${emp.Salary})");
            }

            return sb.ToString();
        }

        public static string DeleteProjectById(SoftUniContext context)
        {
            var project = context.Projects?.Find(2);

            var emp = context.EmployeesProjects
                .Include(x => x.Project)
                .Where(x => x.ProjectId == 2)
                .ToList();

            context.EmployeesProjects.RemoveRange(emp);

            if (project != null)
            {

                context.Projects.Remove(project);

            }

            context.SaveChanges();


            var projectsToPrint = context.Projects
                .Take(10)
                .Select(x => new
                {
                    Name = x.Name
                })
                .ToList();

            var sb = new StringBuilder();

            foreach (var item in projectsToPrint)
            {
                sb.AppendLine(item.Name);

            }

            return sb.ToString().Trim();
        }

        public static string RemoveTown(SoftUniContext context)
        {


            var town = context.Towns.FirstOrDefault(x => x.Name == "Seetle");



            var addressesId = context.Addresses.
                Where(x => x.TownId == town.TownId)
                .Select(x => x.AddressId)
                .ToList();


            var employees = context.Employees
                .Where(x => x.AddressId.HasValue && addressesId.Any(p => p.Equals(x.AddressId.Value)))
                .ToList();




            foreach (var employee in employees)
            {
                employee.AddressId = null;
            }


            var addresses = context.Addresses.
               Where(x => x.TownId == town.TownId)
               .ToList();

            context.Addresses.RemoveRange(addresses);

            context.Towns.Remove(town);

            context.SaveChanges();

            var output = $"{addresses.Count()} addresses in Seattle were deleted";


            return output;

           


        }



    }
}
