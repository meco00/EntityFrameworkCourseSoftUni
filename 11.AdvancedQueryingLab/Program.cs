using AdvancedQueryingLab.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace AdvancedQueryingLab
{
    class Program
    {
        static void Main(string[] args)
        {
            var db = new SoftUniContext();

            //var critery= "Marketing Specialist";


            //var emp = db.Employees

            //    .FromSqlInterpolated($"SELECT * FROM EMPLOYEES WHERE JOBTITLE = {critery}")
            //    .ToList();

            var ageParam = new SqlParameter("@age", 5);

            db.Database.ExecuteSqlRaw($"EXEC UpdateAge {5}");



          
        }

    }
}
