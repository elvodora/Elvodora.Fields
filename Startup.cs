using Elvodora.Fields.CountryRegion.Drivers;
using Elvodora.Fields.CountryRegion.Fields;
using Elvodora.Fields.CountryRegion.Services;
using Elvodora.Fields.CountryRegion.Settings;
using Elvodora.Fields.CountryRegion.ViewModels;
using Fluid;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentTypes.Editors;
using OrchardCore.Environment.Shell.Configuration;
using OrchardCore.Modules;
using System;

namespace Elvodora.Fields.CountryRegion
{
    public class Startup : StartupBase
    {
        private readonly IShellConfiguration _shellConfiguration;

        public Startup(IShellConfiguration shellConfiguration)
        {
            _shellConfiguration = shellConfiguration;
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            // To be able to access these view models in display shapes rendered by the Liquid markup engine we need to
            // register them. To learn more about Liquid in Orchard Core see this documentation:
            // https://docs.orchardcore.net/en/latest/docs/reference/modules/Liquid/
            services.Configure<TemplateOptions>(options =>
            {
                options.MemberAccessStrategy.Register<CountryRegionField>();
                options.MemberAccessStrategy.Register<DisplayCountryRegionFieldViewModel>();
            });
            // Country Region Service
            services.AddSingleton<ICountryRegionService, CountryRegionService>();
            // Country Region Field
            services.AddContentField<CountryRegionField>()
                .UseDisplayDriver<CountryRegionFieldDisplayDriver>();
            //services.AddScoped<IContentFieldDisplayDriver, CountryRegionFieldDisplayDriver>();
            services.AddScoped<IContentPartFieldDefinitionDisplayDriver, CountryRegionFieldSettingsDriver>();
        }
    }
}
