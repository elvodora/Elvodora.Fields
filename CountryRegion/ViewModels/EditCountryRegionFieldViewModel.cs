using Elvodora.Fields.CountryRegion.Fields;
using Elvodora.Fields.CountryRegion.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Metadata.Models;
using System.Collections.Generic;

namespace Elvodora.Fields.CountryRegion.ViewModels
{
    public class EditCountryRegionFieldViewModel
    {
        public string CountryCode { get; set; }
        public string RegionCodes { get; set; }
        [BindNever]
        public Country Country{ get; set; }
        [BindNever]
        public Region[] Regions { get; set; }
        [BindNever]
        public CountryRegionField Field { get; set; }
        [BindNever]
        public ContentPart Part { get; set; }
        [BindNever]
        public ContentPartFieldDefinition PartFieldDefinition { get; set; }
        [BindNever]
        public ContentTypePartDefinition TypePartDefinition { get; set; }

    }
}
