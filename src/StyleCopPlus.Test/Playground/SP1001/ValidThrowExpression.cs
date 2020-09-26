using System;

namespace AnalyzerPlayground
{
    public class SP1001InvalidExceptionMessageAnalyzer
    {
        public object Main()
        {
            object myval = null;
            return myval ?? throw new Exception("test.");
        }
    }
}