using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.Data.Migration;
using OrchardCore.Commerce.Indexes;

namespace OrchardCore.Commerce.Migrations
{
    /// <summary>
    /// Adds the product part to the list of available parts.
    /// </summary>
    public class ProductMigrations : DataMigration
    {
        IContentDefinitionManager _contentDefinitionManager;

        public ProductMigrations(IContentDefinitionManager contentDefinitionManager)
        {
            _contentDefinitionManager = contentDefinitionManager;
        }

        public int Create()
        {
            _contentDefinitionManager.AlterPartDefinition("ProductPart", builder => builder
                .Attachable()
                .WithDescription("Makes a content item into a product."));

            SchemaBuilder.CreateMapIndexTable(nameof(ProductPartIndex), table => table
                .Column<string>(nameof(ProductPartIndex.Sku), col => col.WithLength(128))
                .Column<string>(nameof(ProductPartIndex.ContentItemId), c => c.WithLength(26))
            );

            SchemaBuilder.AlterTable(nameof(ProductPartIndex), table => table
                .CreateIndex($"IDX_{nameof(ProductPartIndex)}_{nameof(ProductPartIndex.Sku)}", nameof(ProductPartIndex.Sku))
            );

            return 1;
        }
    }
}
