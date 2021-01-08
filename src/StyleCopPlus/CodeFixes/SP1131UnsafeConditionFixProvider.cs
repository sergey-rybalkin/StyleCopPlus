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
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(SP1131UnsafeConditionFixProvider)), Shared]
    public class SP1131UnsafeConditionFixProvider : StyleCopPlusCodeFixProvider
    {
        private const string Title = "Replace with pattern matching";

        /// <summary>
        /// Gets a list of diagnostic IDs that this provider can provider fixes for.
        /// </summary>
        public sealed override ImmutableArray<string> FixableDiagnosticIds =>
            ImmutableArray.Create(SP1131UnsafeConditionAnalyzer.DiagnosticId);

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
        public sealed override Task RegisterCodeFixesAsync(CodeFixContext context)
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

            return Task.CompletedTask;
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
            var model = await document.GetSemanticModelAsync(cancellationToken).ConfigureAwait(false);
            var newBinaryExpression = TransformExpression(binaryExpression, model);

            return document.WithSyntaxRoot(syntaxRoot.ReplaceNode(binaryExpression, newBinaryExpression));
        }

        private static IsPatternExpressionSyntax TransformExpression(
            BinaryExpressionSyntax binaryExpression,
            SemanticModel model)
        {
            bool constOnLeft = SP1131UnsafeConditionAnalyzer.IsLiteral(binaryExpression.Left, model);
            ExpressionSyntax newLeft = constOnLeft ? binaryExpression.Right : binaryExpression.Left;
            ExpressionSyntax newRight = constOnLeft ? binaryExpression.Left : binaryExpression.Right;
            var pattern = GetPatternExpression(binaryExpression.OperatorToken, newRight.WithoutTrivia());

            return SyntaxFactory.IsPatternExpression(newLeft, pattern);
        }

        private static PatternSyntax GetPatternExpression(SyntaxToken @operator, ExpressionSyntax pattern)
        {
            switch (@operator.Kind())
            {
                case SyntaxKind.EqualsEqualsToken:
                    return SyntaxFactory.ConstantPattern(pattern);
                case SyntaxKind.ExclamationEqualsToken:
                    return SyntaxFactory.UnaryPattern(SyntaxFactory.ConstantPattern(pattern));
                default:
                    return SyntaxFactory.ConstantPattern(pattern);
            }
        }
    }
}
