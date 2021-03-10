using LinqLab.Models;
using System;
using System.Linq;

namespace LinqLab
{
    class Program
    {
        static void Main(string[] args)
        {
            var context = new SoftUniContext();

            var employees = context.Employees.Join(context.Departments, (e => e.DepartmentId), (d => d.DepartmentId),
                (e, d) => new
                {
                    Employee = e.FirstName + ' ' + e.LastName,
                    JobTitle = e.JobTitle,
                    Department = d.Name

                }
                ).ToList();


           

            var groupedCustomers = context.Employees.ToList()
                    .GroupBy(employee => employee.JobTitle)
                    .Select(x => new
                    {
                        JobTitle = x.Key,
                        EmployeesFullName = x.Select(x => x.FirstName + ' ' + x.LastName).ToList()



                    }
                     )
                    .Where(x => x.EmployeesFullName.Count > 10)
                    .Take(10);

            var selectManyExample = context.Employees.SelectMany(ed => ed.InverseManager, (manager, employee) => new
            {
                ManagerName = manager.FirstName + ' ' + manager.LastName,
                EmployeeName = employee.FirstName + ' ' + employee.LastName
            })

                .ToList();


           
                 





         


            foreach (var employee in selectManyExample)
            {

               

                Console.WriteLine($"{employee.ManagerName} is manager of  => {employee.EmployeeName}");
                

                
            }
        }
    }
}
