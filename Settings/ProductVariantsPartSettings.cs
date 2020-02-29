using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace OrchardCore.Commerce.Settings
{
    public class ProductVariantsPartSettings
    {
        [DefaultValue("{{ ContentItem.DisplayText }}")]
        public string Pattern { get; set; } = "{{ ContentItem.DisplayText }}";
    }
}
