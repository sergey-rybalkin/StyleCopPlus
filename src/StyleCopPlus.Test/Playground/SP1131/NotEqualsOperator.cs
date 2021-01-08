namespace AnalyzerPlayground
{
    public class NotEqualsOperator
    {
        public void Test(int i)
        {
            if (/*C*/i != 1)
               i = 1;
        }
    }
}
