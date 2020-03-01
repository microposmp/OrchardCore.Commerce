using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrchardCore.Commerce.Abstractions;
using OrchardCore.Commerce.Indexes;
using OrchardCore.Commerce.Models;
using OrchardCore.ContentManagement;
using OrchardCore.Flows.Models;
using YesSql;
using YesSql.Services;

namespace OrchardCore.Commerce.Services
{
    public class ProductVariantsService : IProductService
    {
        private readonly ISession _session;
        private readonly IContentManager _contentManager;

        public ProductVariantsService(
            ISession session,
            IContentManager contentManager)
        {
            _session = session;
            _contentManager = contentManager;
        }

        public async Task<ProductPart> GetProduct(string sku)
        {
            var contentItemId = (await _session.QueryIndex<ProductPartIndex>(x => x.Sku == sku).FirstOrDefaultAsync())?.ContentItemId;
            var product = contentItemId is null ? null : (await _contentManager.GetAsync(contentItemId));

            // If base product has a ProductPart return it.
            if (product.As<ProductPart>() != null)
                return product.As<ProductPart>();

            var skuProduct = product.As<BagPart>()?.ContentItems.Select(item => item.As<ProductPart>()).Where(x => x.Sku == sku).FirstOrDefault();

            return skuProduct;
        }

        public async Task<IEnumerable<ProductPart>> GetProducts(IEnumerable<string> skus)
        {
            List<ProductPart> products = new List<ProductPart>();

            foreach (var sku in skus)
            {
                var contentItemId = (await _session.QueryIndex<ProductPartIndex>(x => x.Sku == sku).FirstOrDefaultAsync())?.ContentItemId;
                var product = contentItemId is null ? null : (await _contentManager.GetAsync(contentItemId));

                // If base product has a ProductPart return it.
                if (product.As<ProductPart>() != null)
                    products.Add(product.As<ProductPart>());

                var skuProduct = product.As<BagPart>()?.ContentItems.Select(item => item.As<ProductPart>()).Where(x => x.Sku == sku).FirstOrDefault();

                if (skuProduct != null)
                    products.Add(skuProduct);
            }

            return products;
        }
    }
}
