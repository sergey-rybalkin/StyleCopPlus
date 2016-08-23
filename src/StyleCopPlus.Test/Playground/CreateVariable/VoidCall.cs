using System.IO;

namespace AnalyzerPlayground.CreateVariable
{
    public class VoidCall
    {
        public void TestMethodCall()
        {
            // test

            TestMethodCall/*C*/();

            // test
        }
    }
}