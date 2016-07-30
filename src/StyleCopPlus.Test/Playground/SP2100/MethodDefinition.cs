namespace AnalyzerPlayground
{
    public class SP2100MethodDefinition
    {
        public SP2100MethodDefinition()
        {
        }

        /// <summary>
        /// Method with very long parameters list.
        /// </summary>
        /// <param name="parameter1">The first parameter.</param>
        /// <param name="parameter2">The second parameter.</param>
        /// <param name="parameter3">The third parameter.</param>
        /// <param name="parameter4">The fourth parameter.</param>
        public static int MethodWithVeryLongParametersList(string parameter1, string parameter2, int parameter3, long parameter4)
        {
            return 1;
        }
    }
}