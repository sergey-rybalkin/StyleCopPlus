using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StyleCopPlus.Analyzers;
using StyleCopPlus.Test.Helpers;
using TestHelper;

namespace StyleCopPlus.Test.Analyzers
{
    [TestClass]
    public class SP1003AnalyzerTests : DiagnosticVerifier
    {
        [TestMethod]
        public void DoesNotReport_ValidReturnStatements()
        {
            string test = DataHelper.GetEmbeddedResource(
                DataHelper.SP1003FormattedReturnStatements,
                out _,
                out _);

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        public void Reports_UnformattedBlockReturnStatement()
        {
            string test = DataHelper.GetEmbeddedResource(
                DataHelper.SP1003UnformattedBlockReturnStatement,
                out int line,
                out int column);

            DiagnosticResult expected = CreateResult(line, column);

            VerifyCSharpDiagnostic(test, expected);
        }

        [TestMethod]
        public void Reports_UnformattedMethodReturnStatement()
        {
            string test = DataHelper.GetEmbeddedResource(
                DataHelper.SP1003UnformattedMethodReturnStatement,
                out int line,
                out int column);

            DiagnosticResult expected = CreateResult(line, column);

            VerifyCSharpDiagnostic(test, expected);
        }

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new SP1003SeparatedReturnStatementAnalyzer();
        }

        private DiagnosticResult CreateResult(int lineNumber, int column)
        {
            return new DiagnosticResult
            {
                Id = SP1003SeparatedReturnStatementAnalyzer.DiagnosticId,
                Message = SP1003SeparatedReturnStatementAnalyzer.MessageFormat,
                Severity = DiagnosticSeverity.Warning,
                Locations =
                [
                     new DiagnosticResultLocation("Test0.cs", lineNumber, column)
                ]
            };
        }

    }
}
