namespace AnalyzerPlayground.CreateVariable
{
    public class FluentApiCalls
    {
        public void TestMethodCall()
        {
            // test

            M1/*C*/().M2();

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