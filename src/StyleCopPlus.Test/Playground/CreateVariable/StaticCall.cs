using System.IO;

namespace AnalyzerPlayground.CreateVariable
{
    public class StaticCall
    {
        public void TestMethodCall()
        {
            // test

            File.ReadAllText/*C*/(@"c:\temp\test.txt");

            // test
        }
    }
}