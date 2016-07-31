using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeRefactorings;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestHelper;

namespace StyleCopPlus.Test.Verifiers
{
    /// <summary>
    /// Base class for all code refactoring unit tests containers.
    /// </summary>
    public abstract class CodeRefactoringVerifier : DiagnosticVerifier
    {
        protected abstract CodeRefactoringProvider CreateProvider();

        protected void VerifyRefactoring(string original, string expected, TextSpan cursorPos)
        {
            Document document = CreateDocument(original);

            var builder = ImmutableArray.CreateBuilder<CodeAction>();
            Action<CodeAction> registerRefactoring = a => builder.Add(a);
            var provider = CreateProvider();
            var context = new CodeRefactoringContext(
                document,
                cursorPos,
                registerRefactoring,
                CancellationToken.None);

            provider.ComputeRefactoringsAsync(context).Wait();
            ImmutableArray<CodeAction> refactorings = builder.ToImmutable();

            Assert.AreEqual(1, refactorings.Length);

            var operations = refactorings[0].GetOperationsAsync(CancellationToken.None).Result;

            var operation = operations.Single();
            var workspace = document.Project.Solution.Workspace;
            operation.Apply(workspace, CancellationToken.None);
            var newDocument = workspace.CurrentSolution.GetDocument(document.Id);
            var sourceText = newDocument.GetTextAsync(CancellationToken.None).Result;
            var text = sourceText.ToString();

            string normalizedText = text.Replace("\r\n", "\n").Replace("\t", "    ");
            string normalizedExpected = expected.Replace("\r\n", "\n").Replace("\t", "    ");

            Assert.AreEqual(normalizedText, normalizedExpected);
        }
    }
}