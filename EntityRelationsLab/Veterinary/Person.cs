using System;
using System.Collections.Generic;
using System.Text;

namespace EntityRelationsLab
{
    public class Person
    {
        public Person()
        {
            Pets = new HashSet<Pet>();
        }
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public DateTime LastUpdated { get; set; }

        public virtual ICollection<Pet> Pets { get; set; }

    }
}
