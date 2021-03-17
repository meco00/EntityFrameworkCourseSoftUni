using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarDealer.DTO
{
    public class CarInputDTO
    {
        //    "make": "Opel",
        //"model": "Astra",
        //"travelledDistance": 516628215,
        //"partsId": [
        //  39,
        //  62,
        //  72
        //]

        [JsonProperty("make")]
        public string Make { get; set; }

        [JsonProperty("model")]
        public string Model { get; set; }

        [JsonProperty("travelledDistance")]
        public long TravelledDistance { get; set; }

        [JsonProperty("partsId")]
        public List<int> PartsId { get; set; }


    }
}
