using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StyleCopPlus.Analyzers;
using StyleCopPlus.Test.Helpers;
using TestHelper;

namespace StyleCopPlus.Test.Analyzers
{
    [TestClass]
    public class SP1004AnalyzerTests : DiagnosticVerifier
    {
        [TestMethod]
        public void DoesNotReport_ReturnOnOwnLine()
        {
            string test = DataHelper.GetEmbeddedResource(
                DataHelper.SP1004ReturnOnOwnLine,
                out _,
                out _);

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        public void Reports_ReturnNotOnOwnLine()
        {
            string test = DataHelper.GetEmbeddedResource(
                DataHelper.SP1004ReturnNotOnOwnLine,
                out int line,
                out int column);

            DiagnosticResult expected = CreateResult(line, column);

            VerifyCSharpDiagnostic(test, expected);
        }

        [TestMethod]
        public void Reports_ReturnAfterStatementSameLine()
        {
            string test = DataHelper.GetEmbeddedResource(
                DataHelper.SP1004ReturnAfterStatementSameLine,
                out int line,
                out int column);

            DiagnosticResult expected = CreateResult(line, column);

            VerifyCSharpDiagnostic(test, expected);
        }

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new SP1004ReturnOnOwnLineAnalyzer();
        }

        private DiagnosticResult CreateResult(int lineNumber, int column)
        {
            return new DiagnosticResult
            {
                Id = SP1004ReturnOnOwnLineAnalyzer.DiagnosticId,
                Message = SP1004ReturnOnOwnLineAnalyzer.MessageFormat,
                Severity = DiagnosticSeverity.Warning,
                Locations =
                [
                     new DiagnosticResultLocation("Test0.cs", lineNumber, column)
                ]
            };
        }

    }
}
