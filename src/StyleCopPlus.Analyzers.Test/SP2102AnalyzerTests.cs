using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StyleCopPlus.Analyzers.Test.Helpers;
using TestHelper;

namespace StyleCopPlus.Analyzers.Test
{
    [TestClass]
    public class SP2102AnalyzerTests : DiagnosticVerifier
    {
        [TestMethod]
        public void DoesNotReport_SmallProperties()
        {
            var test = DataHelper.GetEmbeddedResource(DataHelper.SP2102ValidClass);

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        public void Reports_LongGetter()
        {
            string test = DataHelper.GetEmbeddedResource(DataHelper.SP2102LongGetter);
            DiagnosticResult expected = CreateResult(132, 5);

            VerifyCSharpDiagnostic(test, expected);
        }

        [TestMethod]
        public void Reports_LongSetter()
        {
            string test = DataHelper.GetEmbeddedResource(DataHelper.SP2102LongSetter);
            DiagnosticResult expected = CreateResult(132, 5);

            VerifyCSharpDiagnostic(test, expected);
        }

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new SP2102Analyzer();
        }

        private DiagnosticResult CreateResult(int linesCount, int lineNumber)
        {
            return new DiagnosticResult
            {
                Id = SP2102Analyzer.DiagnosticId,
                Message = string.Format(
                    SP2102Analyzer.MessageFormat,
                    Settings.SP2102MaxPropertyAccessorLength,
                    linesCount),
                Severity = DiagnosticSeverity.Warning,
                Locations = new[]
                {
                     new DiagnosticResultLocation("Test0.cs", lineNumber, 0)
                }
            };
        }
    }
}