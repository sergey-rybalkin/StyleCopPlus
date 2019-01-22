using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StyleCopPlus.Analyzers;
using StyleCopPlus.Test.Helpers;
using TestHelper;

namespace StyleCopPlus.Test.Analyzers
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
            int line, column;
            string test = DataHelper.GetEmbeddedResource(DataHelper.SP2102LongGetter, out line, out column);
            DiagnosticResult expected =
                CreateResult(Settings.SP2102MaxPropertyAccessorLengthDefault + 1, line, column);

            VerifyCSharpDiagnostic(test, expected);
        }

        [TestMethod]
        public void Reports_LongSetter()
        {
            int line, column;
            string test = DataHelper.GetEmbeddedResource(DataHelper.SP2102LongSetter, out line, out column);
            DiagnosticResult expected =
                CreateResult(Settings.SP2102MaxPropertyAccessorLengthDefault + 1, line, column);

            VerifyCSharpDiagnostic(test, expected);
        }

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new SP2102PropertyTooLongAnalyzer();
        }

        private DiagnosticResult CreateResult(int linesCount, int lineNumber, int column)
        {
            return new DiagnosticResult
            {
                Id = SP2102PropertyTooLongAnalyzer.DiagnosticId,
                Message = string.Format(
                    SP2102PropertyTooLongAnalyzer.MessageFormat,
                    Settings.SP2102MaxPropertyAccessorLengthDefault,
                    linesCount),
                Severity = DiagnosticSeverity.Warning,
                Locations = new[]
                {
                     new DiagnosticResultLocation("Test0.cs", lineNumber, column)
                }
            };
        }
    }
}
