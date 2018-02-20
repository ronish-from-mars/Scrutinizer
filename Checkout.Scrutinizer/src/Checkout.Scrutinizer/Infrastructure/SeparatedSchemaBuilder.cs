namespace Checkout.Scrutinizer.Infrastructure
{
    using FlatFiles.Scrutinizer;

    using Checkout.Scrutinizer.Core;

    public class SeparatedSchemaBuilder: BaseSchemaBuilder, ISchemaBuilder<SeparatedFileSchema>
    {
        public SeparatedFileSchema BuildSchema(RawFileSchema rawSchema)
        {
            var separatedFileSchema = new SeparatedFileSchema();
            var rowDefinitionList = rawSchema.Schema.RowDefinitions;

            separatedFileSchema.Delimeter = rawSchema.Delimeter;

            foreach (var rowDefinition in rowDefinitionList)
            {
                var columnDefinitions = rowDefinition.ColumnDefinitions;
                var rowSchema = new SeparatedValueSchema();

                foreach (var columnDefinition in columnDefinitions)
                {
                    var definition = GetColumnDefinition(columnDefinition);
                    definition.NullHandler = ConstantNullHandler.For("NULL");
                    rowSchema.AddColumn(definition);
                }

                separatedFileSchema.SeparatedRecordSchemas.Add(new SeparatedRecordSchema
                {
                    RecordIdentifier = rowDefinition.Identifier,
                    SeparatedValueSchema = rowSchema
                });
            }

            return separatedFileSchema;
        }
    }
}
