namespace AnalyzerPlayground
{
    public class NotEqualsOperator
    {
        public void Test(int i)
        {
            if (i is not 1)
               i = 1;
        }
    }
}
