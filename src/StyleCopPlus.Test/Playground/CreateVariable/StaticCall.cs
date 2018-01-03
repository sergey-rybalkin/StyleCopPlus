using System.IO;

namespace AnalyzerPlayground.CreateVariable
{
    public class StaticCall
    {
        public void TestMethodCall()
        {
            // test

            string.Format/*C*/("{0}", "str");

            // test
        }
    }
}