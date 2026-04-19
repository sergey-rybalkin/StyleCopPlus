using System;

namespace AnalyzerPlayground
{
    public class C
    {
        public void M()
        {
            throw new InvalidOperationException(message: /*C*/"Error");
        }
    }
|