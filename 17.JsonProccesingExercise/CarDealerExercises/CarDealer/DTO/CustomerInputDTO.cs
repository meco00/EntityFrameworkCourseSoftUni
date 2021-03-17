using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarDealer.DTO
{
    class CustomerInputDTO
    {
        //    "name": "Marcelle Griego",
        //"birthDate": "1990-10-04T00:00:00",
        //"isYoungDriver": true

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("birthDate")]
        public DateTime BirthDate { get; set; }

        [JsonProperty("isYoungDriver")]
        public bool IsYoungDriver { get; set; }
    }
}
