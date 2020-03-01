using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrchardCore.Commerce.Abstractions;
using OrchardCore.Commerce.Indexes;
using OrchardCore.Commerce.Models;
using OrchardCore.ContentManagement;
using OrchardCore.Flows.Models;
using YesSql;

namespace OrchardCore.Commerce.Services
{
    public class SkuPriceVariantsProvider : IPriceProvider
    {
        private readonly IProductService _productService;
        private readonly IMoneyService _moneyService;
        private readonly IContentManager _contentManager;
        private readonly ISession _session;

        public SkuPriceVariantsProvider(
            IProductService productService,
            IMoneyService moneyService,
            IContentManager contentManager,
            ISession session)
        {
            _productService = productService;
            _moneyService = moneyService;
            _contentManager = contentManager;
            _session = session;
        }

        public int Order => 2;

        public async Task AddPrices(IList<ShoppingCartItem> items)
        {
            var skus = items.Select(item => item.ProductSku).Distinct().ToArray();
            var skuProducts = (await _productService.GetProducts(skus))
                .ToDictionary(p => p.Sku);
            foreach (var item in items)
            {
                if (skuProducts.TryGetValue(item.ProductSku, out var product))
                {
                    var contentItem = product.ContentItem;

                    foreach (var pricePart in contentItem.OfType<PricePart>()
                                 .Where(p => p.Price.Currency == _moneyService.CurrentDisplayCurrency))
                    {
                        item.Prices.Add(new PrioritizedPrice(5, pricePart.Price));
                    }

                    // Lookup prices stored in bagpart...
                    var contentItemId = (await _session.QueryIndex<ProductPartIndex>(x => x.Sku == item.ProductSku).FirstOrDefaultAsync())?.ContentItemId;
                    var product2 = contentItemId is null ? null : (await _contentManager.GetAsync(contentItemId));

                    // If base product has a price return it.
                    if (product2.As<PricePart>() != null)
                        item.Prices.Add(new PrioritizedPrice(0, product2.As<PricePart>().Price));

                    var skuProduct = product2.As<BagPart>()?.ContentItems.Select(item => item.As<ProductPart>()).Where(x => x.Sku == item.ProductSku).FirstOrDefault();

                    if (skuProduct != null)
                    {
                        var skuPrice = skuProduct.ContentItem.As<PricePart>();

                        if (skuPrice != null)
                        {
                            item.Prices.Add(new PrioritizedPrice(10, skuPrice.Price));
                        }
                    }
                }
            }
        }
    }
}
