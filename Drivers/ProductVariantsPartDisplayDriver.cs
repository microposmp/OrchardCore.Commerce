using System.Threading.Tasks;
using OrchardCore.Commerce.Abstractions;
using OrchardCore.Commerce.Models;
using OrchardCore.Commerce.ViewModels;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Views;

namespace OrchardCore.Commerce.Drivers
{
    public class ProductVariantsPartDisplayDriver : ContentPartDisplayDriver<ProductVariantsPart>
    {
        private readonly IProductAttributeService _productAttributeService;

        public ProductVariantsPartDisplayDriver(IProductAttributeService productAttributeService)
        {
            _productAttributeService = productAttributeService;
        }

        public override IDisplayResult Display(ProductVariantsPart part, BuildPartDisplayContext context)
        {
            return Initialize<ProductVariantsPartViewModel>(GetDisplayShapeType(context), m => BuildViewModel(m, part))
                .Location("Detail", "Content:20")
                .Location("Summary", "Meta:5");
        }

        public override IDisplayResult Edit(ProductVariantsPart part, BuildPartEditorContext context)
        {
            return Initialize<ProductVariantsPartViewModel>(GetEditorShapeType(context), m => BuildViewModel(m, part));
        }

        public override async Task<IDisplayResult> UpdateAsync(ProductVariantsPart model, IUpdateModel updater, UpdatePartEditorContext context)
        {
            await updater.TryUpdateModelAsync(model, Prefix, t => t.ProductId, t => t.Sku);

            return Edit(model, context);
        }

        private Task BuildViewModel(ProductVariantsPartViewModel model, ProductVariantsPart part)
        {
            model.ContentItem = part.ContentItem;
            model.ProductId = part.ProductId;
            model.Sku = part.Sku;
            model.ProductVariantsPart = part;

            return Task.CompletedTask;
        }
    }
}
