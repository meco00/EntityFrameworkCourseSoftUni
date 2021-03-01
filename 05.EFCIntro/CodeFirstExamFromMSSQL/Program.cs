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

           


            var commits = context.Commits
                .Include(x => x.Repository)
                .Include(x=>x.User)
                .Include(x=>x.Files)
                .Include(x=>x.Issue)
                .ToList();
            ;

            var repos = context.Repositories
                  .Include(x => x.Commits)
                  .Include(x => x.Issues)
                  .ToList();

            ;


         
            ;

            






            

            


           

        }
    }
}
