using System.IO;

namespace AnalyzerPlayground.Refactorings
{
    public class CreateVariablePlayground
    {
        public void TestMethodCall()
        {
            // test

            File.ReadAllText(@"c:\temp\test.txt");

            // test
        }
    }
}