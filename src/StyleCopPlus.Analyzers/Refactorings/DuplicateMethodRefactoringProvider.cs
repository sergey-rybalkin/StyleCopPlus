using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeRefactorings;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace StyleCopPlus.Analyzers.Refactorings
{
    /// <summary>
    /// Provides duplicate method refactorings.
    /// </summary>
    [ExportCodeRefactoringProvider(LanguageNames.CSharp, Name = "DuplicateMethod")]
    public class DuplicateMethodRefactoringProvider :
        SyntaxNodeRefactoringProviderBase<MethodDeclarationSyntax>
    {
        protected override IEnumerable<CodeAction> GetActions(
            Document document,
            SemanticModel semanticModel,
            SyntaxNode root,
            TextSpan span,
            MethodDeclarationSyntax node,
            CancellationToken cancellationToken)
        {
            // Make duplicate method action available only on method identifier
            if (!node.Identifier.Span.IntersectsWith(span))
                yield break;

            yield return CodeAction.Create("Duplicate method", t => DuplicateNode(node, root, document));
        }

        private static Task<Document> DuplicateNode(SyntaxNode node, SyntaxNode root, Document document)
        {
            SyntaxNode updatedNode = node;
            SyntaxTriviaList trivia = node.GetLeadingTrivia();

            // When necessary add additional empty line to separate new method with original
            if (trivia.Count < 2)
            {
                trivia = trivia.Insert(0, SyntaxFactory.CarriageReturnLineFeed);
                updatedNode = node.WithLeadingTrivia(trivia);
            }

            var newRoot = root.InsertNodesAfter(node, new[] { updatedNode });

            return Task.FromResult(document.WithSyntaxRoot(newRoot));
        }
    }
}