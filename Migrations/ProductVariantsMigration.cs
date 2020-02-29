using System;
using System.Collections.Generic;
using System.Text;
using OrchardCore.Commerce.Indexes;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;

namespace OrchardCore.Commerce.Migrations
{
    public class ProductVariantsMigration : DataMigration
    {
        IContentDefinitionManager _contentDefinitionManager;

        public ProductVariantsMigration(IContentDefinitionManager contentDefinitionManager)
        {
            _contentDefinitionManager = contentDefinitionManager;
        }

        public int Create()
        {
            _contentDefinitionManager.AlterPartDefinition("ProductVariantsPart", builder => builder
                .Attachable()
                .WithDescription("Makes a content item into a product with variants."));

            SchemaBuilder.CreateMapIndexTable(nameof(ProductVariantsPartIdIndex), table => table
                .Column<string>("ProductId", col => col.WithLength(128))
                .Column<string>("ContentItemId", col => col.WithLength(26))
            );

            return 1;
        }
    }
}
