using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace StyleCopPlus.Analyzers
{
    /// <summary>
    /// SP1003 rule analyzer - validates that return statement in multiline code block is separated by an
    /// empty line.
    /// </summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class SP1003SeparatedReturnStatementAnalyzer : StyleCopPlusAnalyzer
    {
        public const string DiagnosticId = "SP1003";

        public const string Category = "Readability";

        public const string Title = "Separate return statement with empty line";

        public const string MessageFormat =
            "Ensure that return statement in multiline code block is separated by an empty line";

        public const string Description = "For readability purposes multiline code blocks that end with " +
            "a return statement should separate it with an empty line.";

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
        /// Check whether code block ends with a return statement. 
        /// </summary>
        /// <param name="context">The context.</param>
        private static void HandleStatement(SyntaxNodeAnalysisContext context)
        {
            ReturnStatementSyntax statement = context.Node as ReturnStatementSyntax;
            if (statement is null)
                return;

            ChildSyntaxList siblings = statement.Parent.ChildNodesAndTokens();
            SyntaxNodeOrToken lastItem = siblings.Last();

            // We are interested only in last return statements in multiline code blocks which are always
            // wrapped in braces
            if (lastItem.IsMissing || !lastItem.IsKind(SyntaxKind.CloseBraceToken))
                return;

            // Check whether this return statement is the only statement in the block
            if (siblings.Count(s => !s.IsMissing && s.IsNode) < 2)
                return;

            // Validate that this return statement has an empty line before it
            SyntaxTriviaList leading = statement.GetLeadingTrivia();
            if (!leading.Any(t => t.IsKind(SyntaxKind.EndOfLineTrivia)))
                context.ReportDiagnostic(Diagnostic.Create(Rule, statement.GetLocation()));
        }
    }
}
