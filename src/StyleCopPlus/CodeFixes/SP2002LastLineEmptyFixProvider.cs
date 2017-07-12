using System.Collections.Immutable;
using System.Composition;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using StyleCopPlus.Analyzers;

namespace StyleCopPlus.CodeFixes
{
    /// <summary>
    /// Tries to fix SP2100 warning by splitting long code lines where possible.
    /// </summary>
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(SP2002LastLineEmptyFixProvider)), Shared]
    public class SP2002LastLineEmptyFixProvider : StyleCopPlusCodeFixProvider
    {
        private const string Title = "Remove empty line";

        /// <summary>
        /// Gets a list of diagnostic IDs that this provider can provider fixes for.
        /// </summary>
        public sealed override ImmutableArray<string> FixableDiagnosticIds =>
            ImmutableArray.Create(SP2002LastLineEmptyAnalyzer.DiagnosticId);

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
            foreach (Diagnostic diagnostic in context.Diagnostics)
            {
                TextSpan diagnosticSpan = diagnostic.Location.SourceSpan;
                CodeAction action = CodeAction.Create(
                    Title,
                    c => FormatCodeAsync(context.Document, diagnostic, c),
                    Title);

                context.RegisterCodeFix(action, diagnostic);
            }

            return Task.CompletedTask;
        }

        private async Task<Document> FormatCodeAsync(
            Document document,
            Diagnostic diagnostic,
            CancellationToken cancellationToken)
        {
            int eolTriviaPosition = diagnostic.Location.SourceSpan.Start - 1;
            SyntaxTrivia emptyTrivia = SyntaxFactory.Whitespace(string.Empty);

            SyntaxNode syntaxRoot =
                await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);

            SyntaxTrivia target = syntaxRoot.FindTrivia(eolTriviaPosition, true);
            SyntaxNode newSyntaxRoot = syntaxRoot.ReplaceTrivia(target, emptyTrivia);

            return document.WithSyntaxRoot(newSyntaxRoot);
        }
    }
}