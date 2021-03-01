using P03_SalesDatabase.Data;
using P03_SalesDatabase.Data.Models;
using System;

namespace P03_SalesDatabase
{
    class Program
    {
        static void Main(string[] args)
        {
            var rand = new Random();

            

            var context = new SalesContext();

            //context.Database.EnsureDeleted();
            //context.Database.EnsureCreated();



            for (int i = 50; i < 60; i++)
            {

            var sale = new Sale
            {
                
                Product = new Product()
                {
                    Name = "Sharo"+i,
                    Quantity = 5,
                    Price = 20,
                },
                Customer = new Customer()
                {
                    Name = "Pesho"+i,
                    Email = "zaza@abv.bg"+i,
                    CreditCardNumber = new String(Convert.ToChar(rand.Next(0, 10).ToString()), 10)
                },
                Store = new Store()
                {
                    Name = "Billa"+i
                }


            };

            context.Sales.Add(sale);
            }

            context.SaveChanges();
        }
    }
}
