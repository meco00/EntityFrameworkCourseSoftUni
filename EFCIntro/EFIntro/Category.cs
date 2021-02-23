using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EFIntro
{
    public class Category
    {

        public Category()
        {
            Posts = new HashSet<Post>();
        }


        public int Id { get; set; }

        [Required]
        public string Name { get; set; }


       


        public virtual ICollection<Post> Posts { get; set; }

    }
}