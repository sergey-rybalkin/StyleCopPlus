using System.IO;

namespace AnalyzerPlayground.CreateVariable
{
    public class StaticCall
    {
        public void TestMethodCall()
        {
            // test

            var v = File.ReadAllText(@"c:\temp\test.txt");

            // test
        }
    }
}