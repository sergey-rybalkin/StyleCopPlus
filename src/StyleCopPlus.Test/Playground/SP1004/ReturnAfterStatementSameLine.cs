using System;

namespace AnalyzerPlayground.SP1004
{
    internal class ReturnAfterStatementSameLine
    {
        public int M()
        {
            /*C*/return 1; Console.WriteLine("test");
        }
    }
}
