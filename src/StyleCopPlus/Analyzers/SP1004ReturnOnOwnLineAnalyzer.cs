using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace StyleCopPlus.Analyzers
{
    /// <summary>
    /// SP1004 rule analyzer - validates that return statements are on their own line.
    /// </summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class SP1004ReturnOnOwnLineAnalyzer : StyleCopPlusAnalyzer
    {
        public const string DiagnosticId = "SP1004";

        public const string Category = "Readability";

        public const string Title = "Return statement should be on its own line";

        public const string MessageFormat =
            "Ensure that return statement is on its own line";

        public const string Description = "Return statements should always appear on their own line to " +
            "improve code readability and make debugging easier.";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            DiagnosticId,
            Title,
            MessageFormat,
            Category,
            DiagnosticSeverity.Warning,
            isEnabledByDefault: true,
            description: Description);

        /// <summary>
        /// Gets a set of descriptors for the diagnostics that this analyzer is capable of producing.
        /// </summary>
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
            => ImmutableArray.Create(Rule);

        /// <inheritdoc/>
        protected override void Register(CompilationStartAnalysisContext context, Settings settings)
        {
            context.RegisterSyntaxNodeAction(HandleStatement, SyntaxKind.ReturnStatement);
        }

        /// <summary>
        /// Check whether return statement is on its own line.
        /// </summary>
        /// <param name="context">The context.</param>
        private static void HandleStatement(SyntaxNodeAnalysisContext context)
        {
            ReturnStatementSyntax returnStatement = context.Node as ReturnStatementSyntax;
            if (returnStatement is null)
                return;

            // The newline before the return statement belongs to the trailing trivia of the
            // previous token, not the leading trivia of the return statement itself.
            SyntaxToken previousToken = returnStatement.ReturnKeyword.GetPreviousToken();
            bool hasLeadingNewline = previousToken.IsKind(SyntaxKind.None) ||
                previousToken.TrailingTrivia.Any(t => t.IsKind(SyntaxKind.EndOfLineTrivia));

            // Check if return statement has a newline after it (trailing trivia)
            bool hasTrailingNewline = returnStatement.GetTrailingTrivia()
                .Any(t => t.IsKind(SyntaxKind.EndOfLineTrivia));

            if (!hasLeadingNewline || !hasTrailingNewline)
            {
                context.ReportDiagnostic(Diagnostic.Create(Rule, returnStatement.GetLocation()));
            }
        }
    }
}
