using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarDealer.DTO
{
   public class SupplierInputDTO
    {
        [JsonProperty("name")]
        public string  Name { get;}

        [JsonProperty("isImporter")]
        public bool IsImporter { get;}
    }
}
