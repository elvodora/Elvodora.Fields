using Elvodora.Fields.CountryRegion.Fields;
using Elvodora.Fields.CountryRegion.Models;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Metadata.Models;

namespace Elvodora.Fields.CountryRegion.ViewModels
{
    public class DisplayCountryRegionFieldViewModel
    {
        public string CountryCode => Field.CountryCode; 
        public string[] RegionCodes => Field.RegionCodes;
        public string CountryName { get; set; }
        public Region[] Regions { get; set; }
        public CountryRegionField Field { get; set; }
        public ContentPart Part { get; set; }
        public ContentPartFieldDefinition PartFieldDefinition { get; set; }

    }
}
