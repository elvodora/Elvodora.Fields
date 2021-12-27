namespace Elvodora.Fields.CountryRegion.Settings
{
    public class CountryRegionFieldSettings
    {

        // We'll use this setting to determine whether the value of the field is required to be given by the user.
        public bool Required { get; set; }
        // Hint text to be displayed on the editor.
        public string Hint { get; set; }
        // The label to be used on the editor and the display shape.
        public string Label { get; set; }
        // Determine behaviour how regions of a country will be selected
        public string SelectRegion { get; set; }
    }
}
