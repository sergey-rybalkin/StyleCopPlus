using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StyleCopPlus.Analyzers;
using StyleCopPlus.Test.Helpers;
using TestHelper;

namespace StyleCopPlus.Test.Analyzers
{
    [TestClass]
    public class SP1002AnalyzerTests : DiagnosticVerifier
    {
        [TestMethod]
        public void Reports_InvalidParameterName()
        {
            string test = DataHelper.GetEmbeddedResource(
                DataHelper.SP1002InvalidParameterName,
                out int line,
                out int column);

            DiagnosticResult expected = CreateResult(line, column);

            VerifyCSharpDiagnostic(test, expected);
        }

        [TestMethod]
        public void DoesNotReport_ValidParameterName()
        {
            string test = DataHelper.GetEmbeddedResource(
                DataHelper.SP1002ValidParameterName,
                out _,
                out _);

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        public void DoesNotReport_InheritedParameterName()
        {
            string test = DataHelper.GetEmbeddedResource(
                DataHelper.SP1002InheritedParameterName,
                out int line,
                out int column);

            DiagnosticResult expected = CreateResult(line, column); // Expected on base class

            VerifyCSharpDiagnostic(test, expected);
        }

        [TestMethod]
        public void DoesNotReport_ImplementedParameterName()
        {
            string test = DataHelper.GetEmbeddedResource(
                DataHelper.SP1002ImplementedParameterName,
                out int line,
                out int column);

            DiagnosticResult expected = CreateResult(line, column); // Expected on interface

            VerifyCSharpDiagnostic(test, expected);
        }

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new SP1002CancellationTokenNameAnalyzer();
        }

        private DiagnosticResult CreateResult(int lineNumber, int column)
        {
            string message = string.Format(
                SP1002CancellationTokenNameAnalyzer.MessageFormat,
                "token",
                SP1002CancellationTokenNameAnalyzer.TargetParameterName);

            return new DiagnosticResult
            {
                Id = SP1002CancellationTokenNameAnalyzer.DiagnosticId,
                Message = message,
                Severity = DiagnosticSeverity.Warning,
                Locations =
                [
                     new DiagnosticResultLocation("Test0.cs", lineNumber, column)
                ]
            };
        }

    }
}
