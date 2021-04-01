using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Cinema.DataProcessor.ImportDto
{
    public class ImportHallsSeatsJsonModel
    {
        [Required]
        [StringLength(20,MinimumLength =3)]
        public string Name { get; set; }

        [Required]
        public bool? Is4Dx { get; set; }

        [Required]
        public bool? Is3D { get; set; }

        [Range(0,int.MaxValue)]
        public int Seats { get; set; }
    }

    //"Name": "Methocarbamol",
    //"Is4Dx": false,
    //"Is3D": true,
    //"Seats": 52

    //Name – text with length[3, 20] (required)
    ////•	Is4Dx - bool
    ////•	Is3D - bool

}
