using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EFIntro
{

   [Index(nameof(CategoryId),Name ="IX_CategoryId")]
   public class Post
    {
        public int Id { get; set; }


        [Required]
        [MaxLength(100)]
        public string Content { get; set; }

        

        public DateTime CreatedOn { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }

    }
}
