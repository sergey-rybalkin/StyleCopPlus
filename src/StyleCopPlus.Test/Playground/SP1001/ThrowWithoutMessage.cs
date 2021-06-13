using System;

namespace AnalyzerPlayground
{
    public class SP1001InvalidExceptionMessageAnalyzer
    {
        public void Main()
        {
            throw new Exception(/*C*/);
        }
    }
}