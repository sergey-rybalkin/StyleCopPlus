using System;

namespace AnalyzerPlayground.SP1004
{
    internal class ReturnNotOnOwnLine
    {
        public int M()
        {
            if (DateTime.Now.Second % 2 == 0) /*C*/return 1;

            return 0;
        }
    }
}
