﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StyleCopPlus.Analyzers.Test.Helpers;
using TestHelper;

namespace StyleCopPlus.Analyzers.Test
{
    [TestClass]
    public class SP2101AnalyzerTests : DiagnosticVerifier
    {
        [TestMethod]
        public void DoesNotReport_SmallMethods()
        {
            var test = DataHelper.GetEmbeddedResource(DataHelper.SP2101ValidClass);

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        public void Reports_LongConstructor()
        {
            string test = DataHelper.GetEmbeddedResource(DataHelper.SP2101LongConstructor);
            DiagnosticResult expected = CreateResult(132, 5);

            VerifyCSharpDiagnostic(test, expected);
        }

        [TestMethod]
        public void Reports_LongStaticConstructor()
        {
            string test = DataHelper.GetEmbeddedResource(DataHelper.SP2101LongStaticConstructor);
            DiagnosticResult expected = CreateResult(132, 5);

            VerifyCSharpDiagnostic(test, expected);
        }

        [TestMethod]
        public void Reports_LongMethod()
        {
            string test = DataHelper.GetEmbeddedResource(DataHelper.SP2101LongMethod);
            DiagnosticResult expected = CreateResult(132, 5);

            VerifyCSharpDiagnostic(test, expected);
        }

        [TestMethod]
        public void Reports_LongFinalizer()
        {
            string test = DataHelper.GetEmbeddedResource(DataHelper.SP2101LongFinalizer);
            DiagnosticResult expected = CreateResult(132, 5);

            VerifyCSharpDiagnostic(test, expected);
        }

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new SP2101Analyzer();
        }

        private DiagnosticResult CreateResult(int linesCount, int lineNumber)
        {
            return new DiagnosticResult
            {
                Id = SP2101Analyzer.DiagnosticId,
                Message = string.Format(
                    SP2101Analyzer.MessageFormat,
                    Settings.SP2101MaxMethodLength,
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