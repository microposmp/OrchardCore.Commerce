using System.Threading.Tasks;
using Fluid;
using OrchardCore.Commerce.Abstractions;
using OrchardCore.Commerce.Models;
using OrchardCore.Commerce.Settings;
using OrchardCore.Commerce.ViewModels;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Views;
using OrchardCore.Liquid;

namespace OrchardCore.Commerce.Drivers
{
    public class ProductPartDisplayDriver : ContentPartDisplayDriver<ProductPart>
    {
        private readonly IProductAttributeService _productAttributeService;
        private readonly ILiquidTemplateManager _liquidTemplateManager;

        public ProductPartDisplayDriver(IProductAttributeService productAttributeService, ILiquidTemplateManager liquidTemplateManager)
        {
            _productAttributeService = productAttributeService;
            _liquidTemplateManager = liquidTemplateManager;
        }

        public override IDisplayResult Display(ProductPart productPart, BuildPartDisplayContext context)
        {
            return Initialize<ProductPartViewModel>(GetDisplayShapeType(context), m => BuildViewModel(m, productPart))
                .Location("Detail", "Content:20")
                .Location("Summary", "Meta:5");
        }

        public override IDisplayResult Edit(ProductPart productPart, BuildPartEditorContext context)
        {
            return Initialize<ProductPartViewModel>(GetEditorShapeType(context), m => BuildViewModel(m, productPart));
        }

        public override async Task<IDisplayResult> UpdateAsync(ProductPart model, IUpdateModel updater, UpdatePartEditorContext context)
        {
            if (await updater.TryUpdateModelAsync(model, Prefix, t => t.Sku))
            {
                // Set SKU value only if it's not already set.
                if (string.IsNullOrWhiteSpace(model.Sku))
                {
                    var pattern = context.TypePartDefinition.GetSettings<ProductPartSettings>().Pattern;

                    if (!string.IsNullOrEmpty(pattern))
                    {
                        model.Sku = await _liquidTemplateManager.RenderAsync(pattern, NullEncoder.Default, model.ContentItem, scope => scope.SetValue("ContentItem", model.ContentItem));
                    }
                }
            }

            return Edit(model, context);
        }

        private Task BuildViewModel(ProductPartViewModel model, ProductPart part)
        {
            model.ContentItem = part.ContentItem;
            model.Sku = part.Sku;
            model.ProductPart = part;

            model.Attributes = _productAttributeService.GetProductAttributeFields(part.ContentItem);

            // TODO: filter out of inventory products here as well when we have inventory management
            // model.CanBeBought = ...;

            return Task.CompletedTask;
        }
    }
}
