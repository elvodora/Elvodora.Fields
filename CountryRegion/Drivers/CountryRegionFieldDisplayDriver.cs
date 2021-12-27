using Elvodora.Fields.CountryRegion.Fields;
using Elvodora.Fields.CountryRegion.Models;
using Elvodora.Fields.CountryRegion.Services;
using Elvodora.Fields.CountryRegion.Settings;
using Elvodora.Fields.CountryRegion.ViewModels;
using Microsoft.Extensions.Localization;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.ContentManagement.Metadata.Models;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Views;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Elvodora.Fields.CountryRegion.Drivers
{
    // This class must be registered with the service provider in the Startup.cs.
    // services.AddScoped<IContentFieldDisplayDriver, CountryRegionFieldDisplayDriver>();
    public class CountryRegionFieldDisplayDriver : ContentFieldDisplayDriver<CountryRegionField>
    {
        private readonly IStringLocalizer T;
        private readonly ICountryRegionService _countryRegionService;

        public CountryRegionFieldDisplayDriver(
            IStringLocalizer<CountryRegionFieldDisplayDriver> stringLocalizer,
            ICountryRegionService countryRegionService
            )
        {
            T = stringLocalizer;
            _countryRegionService = countryRegionService;
        }

        // Display method for generating display shapes - the Initialize shape helper is being used.
        // For this we need a view model object which will be populated with the field data.
        // The GetDisplayShapeType helper will generate a conventional
        // shape type for our content field which will be the name of the field.
        // Obviously, alternates can also be used - so if the content item is being displayed
        // with a display type named "Custom" then the CountryRegionField.Custom.cshtml file will be used,
        // otherwise, the CountryRegionField.cshtml will be active.

        public override IDisplayResult Display(CountryRegionField field, BuildFieldDisplayContext fieldDisplayContext) =>
            Initialize<DisplayCountryRegionFieldViewModel>(
                GetDisplayShapeType(fieldDisplayContext),
                viewModel =>
                {
                    viewModel.Field = field;
                    viewModel.Part = fieldDisplayContext.ContentPart;
                    viewModel.PartFieldDefinition = fieldDisplayContext.PartFieldDefinition;
                    if (field.CountryCode != null)
                    {
                        var country = _countryRegionService.GetCountryWithRegions(field.CountryCode);
                        if (country != null)
                        {
                            viewModel.CountryName = country.Name;
                            if (field.RegionCodes != null)
                            {
                                viewModel.Regions = country.Regions.Select(r => new Region { Code = r.Code, Name = r.Name }).Where(r => field.RegionCodes.Contains(r.Code)).ToArray();
                            }
                        }
                    }
                })
                .Location("Detail", "Content")
                .Location("Summary", "Content");

        public override IDisplayResult Edit(CountryRegionField field, BuildFieldEditorContext context) =>
            Initialize<EditCountryRegionFieldViewModel>(
                GetEditorShapeType(context), viewModel =>
                {
                    viewModel.CountryCode = field.CountryCode;
                    viewModel.RegionCodes = string.Join(",", field.RegionCodes);
                    viewModel.Field = field;
                    viewModel.Part = context.ContentPart;
                    viewModel.PartFieldDefinition = context.PartFieldDefinition;
                    if (field.CountryCode != null)
                    {
                        var country = _countryRegionService.GetCountryWithRegions(field.CountryCode);
                        if (country != null)
                        {
                            viewModel.Country = new Country { Code = country.Code, Name = country.Name };
                            if (field.RegionCodes != null)
                            {
                                viewModel.Regions = country.Regions.Select(r => new Region { Code = r.Code, Name = r.Name }).Where(r => field.RegionCodes.Contains(r.Code)).ToArray();
                            }
                        }
                    }
                });

        public override async Task<IDisplayResult> UpdateAsync(CountryRegionField field, IUpdateModel updater, UpdateFieldEditorContext context)
        {
            var viewModel = new EditCountryRegionFieldViewModel();

            if (await updater.TryUpdateModelAsync(viewModel, Prefix, f => f.CountryCode, f => f.RegionCodes))
            {
                field.CountryCode = viewModel.CountryCode;
                field.RegionCodes = viewModel.RegionCodes == null
                    ? Array.Empty<string>() : viewModel.RegionCodes.Split(',', StringSplitOptions.RemoveEmptyEntries);
                // Get the CountryRegionFieldSettings to use it when validating the view model.
                var settings = context.PartFieldDefinition.GetSettings<CountryRegionFieldSettings>();

                if (settings.Required && string.IsNullOrWhiteSpace(viewModel.CountryCode))
                {
                    updater.ModelState.AddModelError(Prefix, T["A value is required for {0}.", context.PartFieldDefinition.DisplayName()]);
                }
                if (settings.Required && settings.SelectRegion != SelectRegionOption.None.ToString() && viewModel.RegionCodes.Length == 0 )
                {
                    updater.ModelState.AddModelError(Prefix, T["At least one region must be selected."]);
                }
            }
            return Edit(field, context);
        }
    }
}
