namespace AnalyzerPlayground.CreateVariable
{
    public class FluentApiCalls
    {
        public void TestMethodCall()
        {
            // test

            int v = M1().M2();

            // test
        }

        private FluentApiCalls M1()
        {
            return this;
        }

        private int M2()
        {
            return 2;
        }
    }
}