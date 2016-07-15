namespace AnalyzerPlayground
{
    public class IncorrectOperands
    {
        public void Test(int i)
        {
            if (1 != i)
               i = 1;
        }
    }
}