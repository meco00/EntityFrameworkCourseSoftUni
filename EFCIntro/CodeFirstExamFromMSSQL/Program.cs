using CodeFirstExamFromMSSQL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace CodeFirstExamFromMSSQL
{
    class Program
    {
        static void Main(string[] args)
        {
            var context = new BitBuckerContext();


            var commits = context.Commits.ToList();

            






            

            


           

        }
    }
}
