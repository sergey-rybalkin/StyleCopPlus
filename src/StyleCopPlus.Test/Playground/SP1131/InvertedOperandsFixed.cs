namespace AnalyzerPlayground
{
    public class IncorrectOperands
    {
        public void Test(int i)
        {
            if (i is 1)
               i = 1;
        }
    }
}
