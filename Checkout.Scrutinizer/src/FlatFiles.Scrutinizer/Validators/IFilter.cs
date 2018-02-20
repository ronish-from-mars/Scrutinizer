using System;
using System.Collections.Generic;
using System.Text;

namespace FlatFiles.Scrutinizer.Validators
{
    public interface IFilter<T>
    {
        T Execute(T input);
    }
}
