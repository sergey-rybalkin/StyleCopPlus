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
        public void Reports_ComparisonWithLiteral()
        {
            string test = DataHelper.GetEmbeddedResource(
                DataHelper.SP1131IncorrectOperands,
                out int line,
                out int column);

            DiagnosticResult expected = CreateResult(line, column);

            VerifyCSharpDiagnostic(test, expected);
        }

        [TestMethod]
        public void Reports_ComparisonWithConstant()
        {
            string test = DataHelper.GetEmbeddedResource(
                DataHelper.SP1131IncorrectOperandsWithConst,
                out int line,
                out int column);

            DiagnosticResult expected = CreateResult(line, column);

            VerifyCSharpDiagnostic(test, expected);
        }

        [TestMethod]
        public void DoesNotReport_ComparisonWithStaticReadonly()
        {
            string test = DataHelper.GetEmbeddedResource(
                DataHelper.SP1131CorrectOperandsWithStatic,
                out _,
                out _);

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        public void DoesNotReport_ComparisonInsideLambda()
        {
            string test = DataHelper.GetEmbeddedResource(
                DataHelper.SP1131OperandWithinLambda,
                out _,
                out _);

            VerifyCSharpDiagnostic(test);
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
