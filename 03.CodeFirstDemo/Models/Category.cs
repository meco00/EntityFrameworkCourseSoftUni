using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CodeFirstDemo.Models
{
    public class Category
    {

        public Category()
        {
            News = new HashSet<News>();
        }
        public int Id { get; set; }


        [MaxLength(100)]
        public string Name { get; set; }


        public virtual ICollection<News> News { get; set; }





    }
}
