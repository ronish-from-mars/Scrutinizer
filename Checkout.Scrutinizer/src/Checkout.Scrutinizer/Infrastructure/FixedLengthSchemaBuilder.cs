namespace Checkout.Scrutinizer.Infrastructure
{
    using FlatFiles.Scrutinizer;

    using Checkout.Scrutinizer.Core;

    public class FixedLengthSchemaBuilder: BaseSchemaBuilder, ISchemaBuilder<FixedLengthFileSchema>
    {
        public FixedLengthFileSchema BuildSchema(RawFileSchema rawSchema)
        {
            var fixedLengthFileSchema = new FixedLengthFileSchema();
            var rowDefinitionList = rawSchema.Schema.RowDefinitions;

            foreach (var rowDefinition in rowDefinitionList)
            {
                var columnDefinitions = rowDefinition.ColumnDefinitions;
                var rowSchema = new FixedLengthSchema();

                foreach (var columnDefinition in columnDefinitions)
                {
                    var definition = GetColumnDefinition(columnDefinition);
                    definition.NullHandler = ConstantNullHandler.For("NULL");
                    rowSchema.AddColumn(
                        definition, BuildWindow(columnDefinition)
                    );
                }

                fixedLengthFileSchema.FixedLengthRecordSchemas.Add(new FixedLengthRecordSchema
                {
                    RecordIdentifier = rowDefinition.Identifier,
                    FixedLengthSchema = rowSchema
                });
            }
            
            return fixedLengthFileSchema;
        }
    }
}
