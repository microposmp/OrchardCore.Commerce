using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using OrchardCore.Commerce.Abstractions;
using OrchardCore.Commerce.Models;
using OrchardCore.ContentManagement;

namespace OrchardCore.Commerce.ViewModels
{
    public class ProductVariantsPartViewModel
    {
        public string ProductId { get; set; }
        public string Sku { get; set; }

        [BindNever]
        public ContentItem ContentItem { get; set; }

        [BindNever]
        public ProductVariantsPart ProductVariantsPart { get; set; }

        [BindNever]
        public IEnumerable<ProductAttributeDescription> Attributes { get; set; }

        [BindNever]
        public bool CanBeBought { get; set; } = true;
    }
}
