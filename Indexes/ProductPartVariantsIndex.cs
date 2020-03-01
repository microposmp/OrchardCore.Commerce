using System.Collections.Generic;
using OrchardCore.Commerce.Models;
using OrchardCore.ContentManagement;
using OrchardCore.Flows.Models;
using YesSql.Indexes;

namespace OrchardCore.Commerce.Indexes
{
    public class ProductPartVariantsIndexProvider : IndexProvider<ContentItem>
    {
        public override void Describe(DescribeContext<ContentItem> context)
        {
            context.For<ProductPartIndex>()
                .Map(contentItem =>
                {
                    var skuBag = contentItem.As<BagPart>();

                    if (!contentItem.IsPublished() || skuBag is null)
                    {
                        return null;
                    }

                    var indexList = new List<ProductPartIndex>();

                    foreach (var sku in skuBag.ContentItems)
                    {
                        var productPart = sku.As<ProductPart>();

                        if (productPart?.Sku == null)
                        {
                            return null;
                        }

                        indexList.Add(new ProductPartIndex()
                        {
                            Sku = productPart.Sku.ToLowerInvariant(),
                            ContentItemId = contentItem.ContentItemId
                        });
                    }

                    return indexList;
                });
        }
    }
}
