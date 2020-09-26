using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StyleCopPlus.Analyzers;
using StyleCopPlus.Test.Helpers;
using TestHelper;

namespace StyleCopPlus.Test.Analyzers
{
    [TestClass]
    public class SP1001AnalyzerTests : DiagnosticVerifier
    {
        [TestMethod]
        public void Reports_InvalidThrowStatementMessage()
        {
            string test = DataHelper.GetEmbeddedResource(
                DataHelper.SP1001ThrowStatement,
                out int line,
                out int column);

            DiagnosticResult expected = CreateResult(line, column);

            VerifyCSharpDiagnostic(test, expected);
        }

        [TestMethod]
        public void Reports_InvalidThrowExpressionMessage()
        {
            string test = DataHelper.GetEmbeddedResource(
                DataHelper.SP1001ThrowExpression,
                out int line,
                out int column);

            DiagnosticResult expected = CreateResult(line, column);

            VerifyCSharpDiagnostic(test, expected);
        }

        [TestMethod]
        public void DoesNotReport_ValidThrowStatementMessage()
        {
            string test = DataHelper.GetEmbeddedResource(
                DataHelper.SP1001ValidThrowStatement,
                out _,
                out _);

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        public void DoesNotReport_ValidThrowExpressionMessage()
        {
            string test = DataHelper.GetEmbeddedResource(
                DataHelper.SP1001ValidThrowExpression,
                out _,
                out _);

            VerifyCSharpDiagnostic(test);
        }

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new SP1001InvalidExceptionMessageAnalyzer();
        }

        private DiagnosticResult CreateResult(int lineNumber, int column)
        {
            return new DiagnosticResult
            {
                Id = SP1001InvalidExceptionMessageAnalyzer.DiagnosticId,
                Message = SP1001InvalidExceptionMessageAnalyzer.MessageFormat,
                Severity = DiagnosticSeverity.Warning,
                Locations = new[]
                {
                     new DiagnosticResultLocation("Test0.cs", lineNumber, column)
                }
            };
        }
    }
}
