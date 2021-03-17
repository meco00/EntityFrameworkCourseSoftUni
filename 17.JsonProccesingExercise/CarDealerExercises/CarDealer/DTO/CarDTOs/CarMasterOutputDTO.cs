using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarDealer.DTO.CarDTOs
{
   public class CarMasterOutputDTO
    {
        [JsonProperty("car")]
        public CarWithListOfPartsViewDTO Car { get; set; }

        [JsonProperty("parts")]
        public ICollection<PartsViewDTO> Parts { get; set; }
    }
}
