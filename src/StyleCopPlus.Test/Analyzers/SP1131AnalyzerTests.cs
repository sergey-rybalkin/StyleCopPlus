using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StyleCopPlus.Analyzers;
using StyleCopPlus.Test.Helpers;
using TestHelper;

namespace StyleCopPlus.Test.Analyzers
{
    [TestClass]
    public class SP1131AnalyzerTests : DiagnosticVerifier
    {
        [TestMethod]
        public void Reports_IncorrectIfOperands()
        {
            int line, column;
            string test = DataHelper.GetEmbeddedResource(
                DataHelper.SP1131IncorrectOperands,
                out line,
                out column);

            DiagnosticResult expected = CreateResult(line, column);

            VerifyCSharpDiagnostic(test, expected);
        }

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new SP1131UnsafeConditionAnalyzer();
        }

        private DiagnosticResult CreateResult(int lineNumber, int column)
        {
            return new DiagnosticResult
            {
                Id = SP1131UnsafeConditionAnalyzer.DiagnosticId,
                Message = SP1131UnsafeConditionAnalyzer.MessageFormat,
                Severity = DiagnosticSeverity.Warning,
                Locations = new[]
                {
                     new DiagnosticResultLocation("Test0.cs", lineNumber, column)
                }
            };
        }
    }
}