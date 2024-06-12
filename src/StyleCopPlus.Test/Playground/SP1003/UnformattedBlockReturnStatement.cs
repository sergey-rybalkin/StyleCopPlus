namespace AnalyzerPlayground.SP1003
{
    internal class UnformattedBlockReturnStatement
    {
        public int M()
        {
            if (DateTime.Now.Second % 2 == 0)
            {
                Console.WriteLine("test");
                /*C*/return 1;
            }

            return 0;
        }
    }
}
