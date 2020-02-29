using OrchardCore.Commerce.Models;
using OrchardCore.ContentManagement;
using YesSql.Indexes;

namespace OrchardCore.Commerce.Indexes
{
    public class ProductVariantsPartIdIndex : MapIndex
    {
        public string ContentItemId { get; set; }
        public string ProductId { get; set; }
    }

    public class ProductVariantsPartIdIndexProvider : IndexProvider<ContentItem>
    {
        public override void Describe(DescribeContext<ContentItem> context)
        {
            context.For<ProductVariantsPartIdIndex>()
                .Map(contentItem =>
                {
                    if(!contentItem.IsPublished())
                    {
                        return null;
                    }

                    var productVariantsPart = contentItem.As<ProductVariantsPart>();

                    if (productVariantsPart?.ProductId == null)
                    {
                        return null;
                    }

                    return new ProductVariantsPartIdIndex
                    {
                        ProductId = productVariantsPart.ProductId.ToLowerInvariant(),
                        ContentItemId = contentItem.ContentItemId,
                    };
                });
        }
    }
}
