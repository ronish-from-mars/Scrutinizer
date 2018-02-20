﻿using System;

namespace FlatFiles.Scrutinizer
{
    /// <summary>
    ///  Specifies how FlatFiles should quote separated value file fields.
    /// </summary>
    public enum QuoteBehavior
    {
        /// <summary>
        /// FlatFiles will only put quotes around values that need to be quoted.
        /// </summary>
        Default = 0,
        /// <summary>
        /// FlatFiles will put quotes around all values.
        /// </summary>
        AlwaysQuote = 1
    }
}
