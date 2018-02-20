namespace Checkout.Scrutinizer.Infrastructure
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    using FlatFiles.Scrutinizer;

    using Checkout.Scrutinizer.Core;
    using Checkout.Scrutinizer.Core.Extensions;

    public abstract class BaseSchemaBuilder
    {
        protected Window BuildWindow(Core.ColumnDefinition columnDefinition)
        {
            if (!columnDefinition.Length.HasValue)
            {
                throw new NullReferenceException("Length cannot be null");
            }

            var window = new Window(columnDefinition.Length.Value);

            if (columnDefinition.Alignment != null)
            {
                window.Alignment = columnDefinition.Alignment.ToEnum<Alignment>().GetAlignment();
            }

            if (!string.IsNullOrEmpty(columnDefinition.PadChar))
            {
                window.FillCharacter = columnDefinition.PadChar[0];
            }

            if (columnDefinition.TruncateIfExceedFieldLength.HasValue)
            {
                if (columnDefinition.TruncateIfExceedFieldLength.Value)
                {
                    window.TruncationPolicy = OverflowTruncationPolicy.TruncateTrailing;
                }
            }

            if (columnDefinition.AllowedValues != null && columnDefinition.AllowedValues.Any())
            {
                var allowedValues = new Dictionary<string, string>();
                foreach (var item in columnDefinition.AllowedValues)
                {
                    allowedValues.Add(item.Value, item.Description);
                }
                window.AllowedValues = allowedValues;
            }

            return window;
        }

        protected IColumnDefinition GetColumnDefinition(Core.ColumnDefinition columnDefinition)
        {
            return columnDefinition.DataType.ToEnum<DataType>().GetColumnDefinition(columnDefinition);
        }
    }
}
