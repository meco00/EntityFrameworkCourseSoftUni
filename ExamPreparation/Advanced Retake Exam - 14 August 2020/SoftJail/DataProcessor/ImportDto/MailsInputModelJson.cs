using System.ComponentModel.DataAnnotations;

namespace SoftJail.DataProcessor.ImportDto
{
    public class MailsInputModelJson
    {
        [Required]
        public string Description { get; set; }

        [Required]
        public string Sender { get; set; }

        [Required]
        [RegularExpression("^([0-9A-Za-z ]+ str.)$")]
        public string Address { get; set; }


       
    }
//    •	Description– text(required)
//•	Sender – text(required)
//•	Address – text, consisting only of letters, spaces and numbers, which ends with “ str.” (required) (Example: “62 Muir Hill str.“)

}