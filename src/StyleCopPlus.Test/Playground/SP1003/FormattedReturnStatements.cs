using System;

namespace AnalyzerPlayground.SP1003
{
    internal class FormattedReturnStatements
    {
        public int M()
        {
            Console.WriteLine("test");

            if (DateTime.Now.Second % 2 == 0)
            {
                return 1;
            }
            else if (DateTime.Now.Second % 2 == 1)
            {
                Console.Write("test");

                return 2;
            }
            else if (DateTime.Now.Second % 3 == 1)
                return 5;

            Console.WriteLine("test");

            return 0;
        }
    }
}
