using OrchardCore.ContentManagement;

namespace OrchardCore.Commerce.Models
{
    /// <summary>
    /// Product part with support for multiple variants.
    /// Restricted to one variant right now to verify the concept.
    /// </summary>
    public class ProductVariantsPart : ProductPart
    {
        public string ProductId { get; set; }
    }
}
