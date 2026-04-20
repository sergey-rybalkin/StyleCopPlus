using System;

namespace AnalyzerPlayground.SP1004
{
    internal class ReturnOnOwnLine
    {
        public void M()
        {
            return;
        }

        public int M2()
        {
            if (true)
            {
                return 1;
            }
            return 0;
        }
    }
}
