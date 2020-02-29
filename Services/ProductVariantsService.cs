using System.Collections.Generic;
using System.Threading.Tasks;
using OrchardCore.Commerce.Abstractions;
using OrchardCore.Commerce.Indexes;
using OrchardCore.Commerce.Models;
using OrchardCore.ContentManagement;
using YesSql;

namespace OrchardCore.Commerce.Services
{
    public class ProductVariantsService : IProductService
    {
        private ISession _session;
        private IContentManager _contentManager;

        public ProductVariantsService(
            ISession session,
            IContentManager contentManager)
        {
            _session = session;
            _contentManager = contentManager;
        }

        public async Task<ProductPart> GetProduct(string sku)
        {
            var contentItemId = (await _session.QueryIndex<ProductVariantsPartSkuIndex>(x => x.Sku == sku).FirstOrDefaultAsync())?.ContentItemId;
            return contentItemId is null ? null : (await _contentManager.GetAsync(contentItemId)).As<ProductVariantsPart>();
        }

        public Task<IEnumerable<ProductPart>> GetProducts(IEnumerable<string> skus)
        {
            throw new System.NotImplementedException();
        }
    }
}
