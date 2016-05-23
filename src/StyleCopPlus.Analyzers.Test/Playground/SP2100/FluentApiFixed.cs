using System;

namespace AnalyzerPlayground
{
    public class FluentApi
    {
        private static Lazy<FluentApi> _current = new Lazy<FluentApi>(() => new FluentApi(), true);

        public FluentApi Method1(string parameter1, string parameter2)
        {
            return this;
        }

        public FluentApi Method2(string parameter1, string parameter2)
        {
            return this;
        }

        public FluentApi Method3(string parameter1, string parameter2)
        {
            return this;
        }

        public FluentApi Method4(string parameter1, string parameter2)
        {
            return this;
        }

        public static FluentApi Current
        {
            get { return _current.Value; }
        }

        public static void Main()
        {
            FluentApi.Current
                     .Method1(string.Empty, string.Empty)
                     .Method2(string.Empty, string.Empty)
                     .Current
                     .Method3(string.Empty, string.Empty)
                     .Method4(string.Empty, string.Empty);
        }
    }
}