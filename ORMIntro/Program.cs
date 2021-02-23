using ORMIntro.Models;
using System;
using System.Linq;

namespace ORMIntro
{
    class Program
    {
        static void Main(string[] args)
        {
            var db = new SoftUniContext();

            //db.SaveChanges();

            //exec sp_executesql N'SET NOCOUNT ON;
            //INSERT INTO[Towns] ([Name])
            //VALUES(@p0);
            //SELECT[TownID]
            //FROM[Towns]
            //WHERE @@ROWCOUNT = 1 AND[TownID] = scope_identity();
            //',N'@p0 varchar(50)',@p0='Momchilgrad'


            //db.Towns.Add(new Town() { Name = "Momchilgrad" });

            //db.SaveChanges();




            //var proj = db.Employees.GroupBy(x => x.Department.Name).Select(x =>
            //    new
            //    {
            //        Name = x.Key,
            //        Count = x.Count()
            //    });


            //var proj2 = db.Departments.Select(x=>new 
            //{
            //    Name=x.Name,
            //    Count=x.Employees.Count()

            //}
            //).ToList();

            //foreach (var item in proj2)
            //{
            //    Console.WriteLine($"{item.Name}  {item.Count}");

            //}



            //var departments = db.Employees.GroupBy(x => x.Department.DepartmentId).Select(x => new { x.Key, Count = x.Count() }).ToList();


            //foreach (var item in departments)
            //{
            //    Console.WriteLine(item);
            //}

            //var query = from employee in db.Employees
            //            join ep in db.EmployeesProjects
            //                on employee.EmployeeId equals ep.EmployeeId
            //            select new { employee, ep };


            //var projects = db.Employees.GroupBy(x => x.EmployeesProjects.Where(y => y.EmployeeId == x.EmployeeId))
            //    .Select(x => new
            //    {
            //        x.Key,
            //        Count = x.Count()

            //    });






            //var i = db.Employees.Where(x => x.Manager != null).Select(x=> new
            //{
            //    EmployeeName=x.EmployeeId,
            //    ManagerName=x.Manager.EmployeeId

            //}).ToList();




            //foreach (var item in i)
            //{
            //    Console.WriteLine($"{item.EmployeeName} {item.ManagerName}");
            //}




            //            SELECT[e].[EmployeeID] AS[EmployeeId], [e0].[FirstName] AS[EmployeeName], COUNT(*) AS[ProjectCount]
            //FROM[EmployeesProjects] AS[e]
            //INNER JOIN[Employees] AS[e0] ON[e].[EmployeeID] = [e0].[EmployeeID]
            //GROUP BY[e].[EmployeeID], [e0].[FirstName]

            var ep = db.EmployeesProjects.GroupBy(x => new { x.EmployeeId, x.Employee.FirstName })
                .Select(x => new
                {
                    EmployeeId = x.Key.EmployeeId,
                    EmployeeName = x.Key.FirstName,
                    ProjectCount = x.Count()
                });


            Console.WriteLine(ep.Count());
            foreach (var item in ep)
            {
                Console.WriteLine(item);

            }


            var employees = db.Employees.ToList();

            ;
        }
    }
}
