using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeRefactorings;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StyleCopPlus.Test.Helpers;
using TestHelper;

namespace StyleCopPlus.Test.Verifiers
{
    /// <summary>
    /// Base class for all code refactoring unit tests containers.
    /// </summary>
    public abstract class CodeRefactoringVerifier : DiagnosticVerifier
    {
        protected abstract CodeRefactoringProvider CreateProvider();

        protected void VerifyRefactoring(
            string original,
            string expected,
            TextSpan cursorPos,
            int actionIndex = 0)
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

            var operations = refactorings[actionIndex].GetOperationsAsync(CancellationToken.None).Result;

            var operation = operations.Single();
            var workspace = document.Project.Solution.Workspace;
            operation.Apply(workspace, CancellationToken.None);
            var newDocument = workspace.CurrentSolution.GetDocument(document.Id);
            var sourceText = newDocument.GetTextAsync(CancellationToken.None).Result;
            var text = sourceText.ToString();

            string normalizedText = text.Replace("\r\n", "\n").Replace("\t", "    ").Trim();
            string normalizedExpected = expected.Replace("\r\n", "\n").Replace("\t", "    ").Trim();

            Assert.AreEqual(normalizedExpected, normalizedText);
        }

        protected void VerifyRefactoringWithResources(string testKey, string expectedKey, int actionIndex = 0)
        {
            int cursorPosition;
            string test = DataHelper.GetEmbeddedResource(testKey, out cursorPosition);
            string expected = DataHelper.GetEmbeddedResource(expectedKey);
            TextSpan cursor = TextSpan.FromBounds(cursorPosition, cursorPosition);

            VerifyRefactoring(test, expected, cursor, actionIndex);
        }

        protected void VerifyNoRefactoring(string resourceKey)
        {
            int cursorPosition;
            string test = DataHelper.GetEmbeddedResource(
                DataHelper.CreateVariableVoidCall,
                out cursorPosition);

            TextSpan cursorSpan = TextSpan.FromBounds(cursorPosition, cursorPosition);
            Document document = CreateDocument(test);

            var builder = ImmutableArray.CreateBuilder<CodeAction>();
            Action<CodeAction> registerRefactoring = a => builder.Add(a);
            var provider = CreateProvider();
            var context = new CodeRefactoringContext(
                document,
                cursorSpan,
                registerRefactoring,
                CancellationToken.None);

            provider.ComputeRefactoringsAsync(context).Wait();
            ImmutableArray<CodeAction> refactorings = builder.ToImmutable();

            Assert.AreEqual(refactorings.Length, 0);
        }
    }
}
