using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using OrchardCore.Commerce.Models;
using OrchardCore.Commerce.ViewModels;
using OrchardCore.ContentManagement.Metadata.Models;
using OrchardCore.ContentTypes.Editors;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Views;

namespace OrchardCore.Commerce.Settings
{
    public class ProductVariantsSettingsDisplayDriver : ContentTypePartDefinitionDisplayDriver
    {
        private readonly IStringLocalizer<PricePartSettingsDisplayDriver> S;

        public ProductVariantsSettingsDisplayDriver(IStringLocalizer<PricePartSettingsDisplayDriver> localizer)
        {
            S = localizer;
        }

        public override IDisplayResult Edit(ContentTypePartDefinition contentTypePartDefinition, IUpdateModel updater)
        {
            if (!String.Equals(nameof(PricePart), contentTypePartDefinition.PartDefinition.Name))
            {
                return null;
            }

            return Initialize<ProductVariantsPartSettingsViewModel>("ProductVariantsPartSettings_Edit", model =>
            {
                var settings = contentTypePartDefinition.GetSettings<ProductVariantsPartSettings>();

                model.Pattern = settings.Pattern;
            }).Location("Content");
        }

        public override async Task<IDisplayResult> UpdateAsync(ContentTypePartDefinition contentTypePartDefinition, UpdateTypePartEditorContext context)
        {
            if (!String.Equals(nameof(ProductVariantsPart), contentTypePartDefinition.PartDefinition.Name))
            {
                return null;
            }

            var model = new ProductVariantsPartSettingsViewModel();

            await context.Updater.TryUpdateModelAsync(model, Prefix, m => m.Pattern);

            context.Builder.WithSettings(new ProductVariantsPartSettings
            {
                Pattern = model.Pattern
            });

            return Edit(contentTypePartDefinition, context.Updater);
        }
    }
}
