﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SoftJail.DataProcessor.ImportDto
{
    public class ImportPrisonersAndMailsJsonInputModel
    {
        [Required]
        [StringLength(20, MinimumLength = 3)]
        public string FullName { get; set; }

        [Required]
        [RegularExpression("^(The [A-Z][a-z]{2,})$")]
        public string Nickname { get; set; }

        [Required]
        [Range(18,65)]
        public int? Age { get; set; }

        [Required]
        public string IncarcerationDate { get; set; }

        public string ReleaseDate { get; set; }

        [Range(typeof(decimal), "0", "79228162514264337593543950335")]
        public decimal? Bail { get; set; }

        public int? CellId { get; set; }

        public ICollection<MailsInputModelJson> Mails { get; set; }
    }

    //    •	FullName – text with min length 3 and max length 20 (required)
    //•	Nickname – text starting with "The " and a single word only of letters with an uppercase letter for beginning(example: The Prisoner) (required)
    //•	Age – integer in the range[18, 65] (required)
    //•	IncarcerationDate ¬– Date(required)
    //•	ReleaseDate– Date
    //•	Bail– decimal (non-negative, minimum value: 0)
    //•	CellId - integer, foreign key


    //"FullName": "",
    //"Nickname": "The Wallaby",
    //"Age": 32,
    //"IncarcerationDate": "29/03/1957",
    //"ReleaseDate": "27/03/2006",
    //"Bail": null,
    //"CellId": 5,


}
