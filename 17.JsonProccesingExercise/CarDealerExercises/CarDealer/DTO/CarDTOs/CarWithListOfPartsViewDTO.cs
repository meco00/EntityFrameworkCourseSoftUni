using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarDealer.DTO.CarDTOs
{
   public class CarWithListOfPartsViewDTO
    {
      

        public string Make { get; set; }

        public string Model { get; set; }

        public long TravelledDistance { get; set; }

        [JsonIgnore]
        public ICollection<PartsViewDTO> Parts { get; set; }



    }
}
