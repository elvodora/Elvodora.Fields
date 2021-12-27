using System.Collections.Generic;

namespace Elvodora.Fields.CountryRegion.Models
{
    public class Country
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public Region[] Regions { get; set; }
    }
}
