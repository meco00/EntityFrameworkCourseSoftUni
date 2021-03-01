using CodeFirstDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeFirstDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var newDb = new ApplicationDbContext();
            newDb.Database.EnsureCreated();


            newDb.Categories.Add
                (
                new Category()
                {
                    Name = "Sport",
                    News = new List<News>()
                    { new News()

                    {
                        Title="ZAzazzuzuz",
                        Context="Mujussad",
                        Comments=new List<Comment>()
                        {
                            new Comment()
                            {
                                Author="Niki",
                                Content="Zazazaza"
                            },
                            new Comment()
                            {
                                Author="Stoyan",
                                Content="Mimimoasaxs"
                            }
                        }
                    }

                    }

                }
                ) ;

            newDb.SaveChanges();


            var something = newDb.Categories.First(x => x.Name.StartsWith("S"));

            something.Name = "MortalCombat";

            newDb.SaveChanges();



        }
    }
}
