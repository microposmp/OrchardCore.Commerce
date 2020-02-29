using OrchardCore.Commerce.Models;
using OrchardCore.ContentManagement;
using YesSql.Indexes;

namespace OrchardCore.Commerce.Indexes
{
    public class ProductVariantsPartSkuIndex : MapIndex
    {
        public string ContentItemId { get; set; }
        public string Sku { get; set; }
    }

    public class ProductVariantPartSkuIndexProvider : IndexProvider<ContentItem>
    {
        public override void Describe(DescribeContext<ContentItem> context)
        {
            context.For<ProductVariantsPartSkuIndex>()
                .Map(contentItem =>
                {
                    if (!contentItem.IsPublished())
                    {
                        return null;
                    }

                    var productVariantsPart = contentItem.As<ProductVariantsPart>();

                    if (productVariantsPart?.Sku is null)
                    {
                        return null;
                    }

                    return new ProductVariantsPartSkuIndex
                    {
                        Sku = productVariantsPart.Sku.ToLowerInvariant(),
                        ContentItemId = contentItem.ContentItemId,
                    };
                });
        }
    }
}
