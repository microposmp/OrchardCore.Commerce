using System.ComponentModel;

namespace OrchardCore.Commerce.Settings
{
    public class ProductPartSettings
    {
        [DefaultValue("{{ Model.ContentItem.TestProduct.ProductId.Text }}")]
        public string Pattern { get; set; } = "{{ Model.ContentItem.TestProduct.ProductId.Text }}";
    }
}
