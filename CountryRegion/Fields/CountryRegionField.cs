using OrchardCore.ContentManagement;
using System;

namespace Elvodora.Fields.CountryRegion.Fields
{
    public class CountryRegionField : ContentField
    {
        public string CountryCode { get; set; }
        public string[] RegionCodes { get; set; } = Array.Empty<string>();
    }
}
