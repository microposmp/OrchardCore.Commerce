using Microsoft.AspNetCore.Mvc.ModelBinding;
using OrchardCore.Commerce.Settings;

namespace OrchardCore.Commerce.ViewModels
{
    public class ProductPartSettingsViewModel
    {
        public string Pattern { get; set; }

        [BindNever]
        public ProductPartSettings ProductPartSettings { get; set; }
    }
}
