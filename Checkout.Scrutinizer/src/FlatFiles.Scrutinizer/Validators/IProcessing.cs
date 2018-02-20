using System;
using System.Collections.Generic;
using System.Text;

namespace FlatFiles.Scrutinizer.Validators
{
    public interface IResult
    {
        bool StopExecution { get; set; }
    }
}
