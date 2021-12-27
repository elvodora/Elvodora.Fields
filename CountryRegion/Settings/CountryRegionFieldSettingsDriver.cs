using Elvodora.Fields.CountryRegion.Fields;
using OrchardCore.ContentManagement.Metadata.Models;
using OrchardCore.ContentTypes.Editors;
using OrchardCore.DisplayManagement.Views;
using System.Threading.Tasks;

namespace Elvodora.Fields.CountryRegion.Settings
{
    // It's in the Settings folder by convention but it's the same DisplayDriver as the others; except, it also has a
    // specific base class. Don't forget to register this class with the service provider (see: Startup.cs).
    public class CountryRegionFieldSettingsDriver : ContentPartFieldDefinitionDisplayDriver<CountryRegionField>
    {
        // This won't have a Display override since it wouldn't make too much sense, settings are just edited.
        public override IDisplayResult Edit(ContentPartFieldDefinition model) =>
            // Same old Initialize shape helper.
            Initialize<CountryRegionFieldSettings>(
                $"{nameof(CountryRegionFieldSettings)}_Edit",
                settings => model.PopulateSettings(settings))
            .Location("Content");

        // CountryRegionFieldSettings.Edit.cshtml file will contain the editor inputs.

        public override async Task<IDisplayResult> UpdateAsync(
            ContentPartFieldDefinition model,
            UpdatePartFieldEditorContext context)
        {
            var settings = new CountryRegionFieldSettings();

            await context.Updater.TryUpdateModelAsync(settings, Prefix);

            // A content field or a content part can have multiple settings. These settings are stored in a single JSON
            // object. This helper will merge our settings into this JSON object so these will be stored.
            context.Builder.WithSettings(settings);
            return Edit(model);
        }
    }
}
