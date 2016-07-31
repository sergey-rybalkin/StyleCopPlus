using System.Collections.Immutable;
using System.Composition;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using StyleCopPlus.Analyzers;

namespace StyleCopPlus.CodeFixes
{
    /// <summary>
    /// Tries to fix SP1131 warning by swapping comparison operands.
    /// </summary>
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(SP1131UnsafeConditionFixProvider))]
    [Shared]
    public class SP1131UnsafeConditionFixProvider : StyleCopPlusCodeFixProvider
    {
        private const string Title = "Swap comparison operands";

        /// <summary>
        /// Gets a list of diagnostic IDs that this provider can provider fixes for.
        /// </summary>
        public sealed override ImmutableArray<string> FixableDiagnosticIds
        {
            get { return ImmutableArray.Create(SP1131UnsafeConditionAnalyzer.DiagnosticId); }
        }

        /// <summary>
        /// Computes one or more fixes for the specified
        /// <see cref="T:Microsoft.CodeAnalysis.CodeFixes.CodeFixContext" />.
        /// </summary>
        /// <param name="context">
        /// A <see cref="T:Microsoft.CodeAnalysis.CodeFixes.CodeFixContext" /> containing context
        /// information about the diagnostics to fix. The context must only contain diagnostics with an
        /// <see cref="P:Microsoft.CodeAnalysis.Diagnostic.Id" /> included in the
        /// <see cref="P:Microsoft.CodeAnalysis.CodeFixes.CodeFixProvider.FixableDiagnosticIds" /> for the
        /// current provider.
        /// </param>
        public override Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            foreach (var diagnostic in context.Diagnostics)
            {
                context.RegisterCodeFix(
                    CodeAction.Create(
                        Title,
                        c => GetTransformedDocumentAsync(context.Document, diagnostic, c),
                        nameof(SP1131UnsafeConditionFixProvider)),
                    diagnostic);
            }

            return CompletedTask;
        }

        private static async Task<Document> GetTransformedDocumentAsync(
            Document document,
            Diagnostic diagnostic,
            CancellationToken cancellationToken)
        {
            var syntaxRoot = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
            var binaryExpression = (BinaryExpressionSyntax)syntaxRoot.FindNode(
                diagnostic.Location.SourceSpan,
                getInnermostNodeForTie: true);

            var newBinaryExpression = TransformExpression(binaryExpression);

            return document.WithSyntaxRoot(syntaxRoot.ReplaceNode(binaryExpression, newBinaryExpression));
        }

        private static BinaryExpressionSyntax TransformExpression(BinaryExpressionSyntax binaryExpression)
        {
            var newLeft = binaryExpression.Right.WithTriviaFrom(binaryExpression.Left);
            var newRight = binaryExpression.Left.WithTriviaFrom(binaryExpression.Right);
            var operatorToken = GetCorrectOperatorToken(binaryExpression.OperatorToken);

            return binaryExpression.WithLeft(newLeft)
                                   .WithRight(newRight)
                                   .WithOperatorToken(operatorToken);
        }

        private static SyntaxToken GetCorrectOperatorToken(SyntaxToken operatorToken)
        {
            switch (operatorToken.Kind())
            {
                case SyntaxKind.EqualsEqualsToken:
                case SyntaxKind.ExclamationEqualsToken:
                    return operatorToken;

                default:
                    return SyntaxFactory.Token(SyntaxKind.None);
            }
        }
    }
}