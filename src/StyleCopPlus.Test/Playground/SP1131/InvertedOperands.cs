namespace AnalyzerPlayground
{
    public class IncorrectOperands
    {
        public void Test(int i)
        {
            if (/*C*/1 == i)
               i = 1;
        }
    }
}
