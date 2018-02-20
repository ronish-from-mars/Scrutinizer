namespace Checkout.Scrutinizer.Core
{
    using System.Collections.Generic;

    public class RowDefinition
    {
        public string Identifier { get; set; }

        public IEnumerable<ColumnDefinition> ColumnDefinitions { get; set; }
    }
}
