using EntityRelationsLab.Veterinary;
using System;
using System.Linq;

namespace EntityRelationsLab
{
    class Program
    {
        static void Main(string[] args)
        {
            var context = new VeterinaryContext();

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();




            for (int i = 1; i < 20; i++)
            {
                var Pet = new Pet()
                {
                    Type = "Pitbull_" + i,
                    ParentId = i

                };

                context.Pets.Add(Pet);

                context.Persons.Add(new Person
                {

                    FirstName = "Moro_" + i,
                    LastName = "Joro_" + i,



                });

            }

            context.SaveChanges();

            var persons = context.Persons
                .Select(x => new
                {
                    FullName = x.FirstName + ' ' + x.LastName,
                    Pets = String.Join(", ", x.Pets.Select(x => x.Type))
                })
                .ToList();

            var firstPerson = context.Persons.FirstOrDefault();

            context.Persons.Remove(firstPerson);
            context.SaveChanges();

            //foreach (var person in persons)
            //{
            //    Console.WriteLine(person.FullName);
            //    Console.WriteLine(person.Pets);

            //}
           
        }
    }
}
