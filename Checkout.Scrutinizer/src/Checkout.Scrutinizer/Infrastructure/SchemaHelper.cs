namespace Checkout.Scrutinizer.Infrastructure
{
    using System;

    using FlatFiles.Scrutinizer;

    using Checkout.Scrutinizer.Core;

    public static class SchemaHelper
    {
        public static IColumnDefinition GetColumnDefinition(this DataType dataType, Core.ColumnDefinition column)
        {
            switch (dataType)
            {
                case DataType.String:
                    return new StringColumn(column.Name);

                case DataType.DateTime:
                    {
                        var datetime = new DateTimeColumn(column.Name);
                        datetime.InputFormat = column.Format;
                        return datetime;
                    }

                case DataType.Int:
                    return new Int64Column(column.Name);

                default:
                    throw new ArgumentOutOfRangeException(nameof(dataType), dataType, null);
            }
        }

        public static FixedAlignment GetAlignment(this Alignment alignment)
        {
            switch (alignment)
            {
                case Alignment.Left:
                    return FixedAlignment.LeftAligned;

                case Alignment.Right:
                    return FixedAlignment.RightAligned;

                default:
                    throw new ArgumentOutOfRangeException(nameof(alignment), alignment, null);
            }
        }
    }
}
