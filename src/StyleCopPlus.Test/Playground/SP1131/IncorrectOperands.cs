namespace AnalyzerPlayground
{
    public class IncorrectOperands
    {
        public void Test(int i)
        {
            if (/*C*/i != 1)
               i = 1;
        }
    }
}