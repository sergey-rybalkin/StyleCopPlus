using System;

namespace AnalyzerPlayground.CheckParameters
{
    class GenericParameter
    {
        public void TestMethod(Tuple<int> param1)
        {
            Verify.ArgumentNotNull(param1);
        }
    }
}
