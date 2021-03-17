using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace JsonProcessingLab
{
    class Program
    {
        static void Main(string[] args)
        {

            var collection = new List<Person>();

            for (int i = 0; i < 10; i++)
            {
                var person = new Person()
                {
                    
                    FirstName = "Ivo"+i


                };

                collection.Add(person);


            }



            var json = JsonSerializer.Serialize(collection);

            Console.WriteLine(json);

            string path = "../../../text.txt";

            File.WriteAllText(path, json);

            string jsonString = File.ReadAllText(path);

            var jason = JsonSerializer.Deserialize<List<Person>>(jsonString);


            ;





        }

        public class Person
        {
            public DateTime BirthDate { get; set; } = DateTime.UtcNow;

            public string FirstName { get; set; } = "Ivo";

            public string LastName { get; set; } = "Pesho";

            public int[] Age { get; set; } = new int[] { 1, 2, 3, 4, 5 };
        }
    }
}
