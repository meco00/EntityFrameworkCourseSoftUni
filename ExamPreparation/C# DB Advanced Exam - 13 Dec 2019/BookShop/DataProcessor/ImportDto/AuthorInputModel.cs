using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BookShop.DataProcessor.ImportDto
{
   public class AuthorInputModel
    {
        [Required]
        [StringLength(30,MinimumLength =3)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 3)]
        public string LastName { get; set; }

        [Required]
        [RegularExpression(@"^(\d{3}-\d{3}-\d{4})$")]
        public string Phone { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public ICollection<AuthorBookInputModel> Books { get; set; }

    }
  

}
