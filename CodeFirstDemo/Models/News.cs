using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CodeFirstDemo.Models
{
    public class News
    {

        public News()
        {
            Comments = new HashSet<Comment>();
        }

        public int Id { get; set; }


        [MaxLength(300)]
        public string Title { get; set; }


        public string Context { get; set; }




        public int CategoryId { get; set; }


        public Category Category { get; set; }


        public  virtual ICollection<Comment> Comments { get; set; }
    }

}
