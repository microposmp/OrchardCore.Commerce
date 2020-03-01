using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using OrchardCore.Commerce.Models;
using OrchardCore.Commerce.ViewModels;
using OrchardCore.ContentManagement.Metadata.Models;
using OrchardCore.ContentTypes.Editors;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Views;
using OrchardCore.Liquid;

namespace OrchardCore.Commerce.Settings
{
    public class ProductPartSettingsDisplayDriver : ContentTypePartDefinitionDisplayDriver
    {
        private readonly ILiquidTemplateManager _templateManager;
        private readonly IStringLocalizer<ProductPartSettingsDisplayDriver> S;

        public ProductPartSettingsDisplayDriver(ILiquidTemplateManager templateManager, IStringLocalizer<ProductPartSettingsDisplayDriver> localizer)
        {
            _templateManager = templateManager;
            S = localizer;
        }

        public override IDisplayResult Edit(ContentTypePartDefinition contentTypePartDefinition, IUpdateModel updater)
        {
            if (!string.Equals(nameof(ProductPart), contentTypePartDefinition.PartDefinition.Name))
            {
                return null;
            }

            return Initialize<ProductPartSettingsViewModel>("ProductPartSettings_Edit", model =>
            {
                var settings = contentTypePartDefinition.GetSettings<ProductPartSettings>();

                model.Pattern = settings.Pattern;
                model.ProductPartSettings = settings;
            }).Location("Content");
        }

        public override async Task<IDisplayResult> UpdateAsync(ContentTypePartDefinition contentTypePartDefinition, UpdateTypePartEditorContext context)
        {
            if (!string.Equals(nameof(ProductPart), contentTypePartDefinition.PartDefinition.Name))
            {
                return null;
            }

            var model = new ProductPartSettingsViewModel();

            await context.Updater.TryUpdateModelAsync(model, Prefix, m => m.Pattern);

            if (!string.IsNullOrEmpty(model.Pattern) && !_templateManager.Validate(model.Pattern, out var errors))
            {
                context.Updater.ModelState.AddModelError(nameof(model.Pattern), S["Pattern doesn't contain a valid Liquid expression. Details: {0}", string.Join(" ", errors)]);
            }
            else
            {
                context.Builder.WithSettings(new ProductPartSettings
                {
                    Pattern = model.Pattern,
                });
            }

            return Edit(contentTypePartDefinition, context.Updater);
        }
    }
}
