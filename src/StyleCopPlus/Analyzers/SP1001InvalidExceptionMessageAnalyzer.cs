using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace StyleCopPlus.Analyzers
{
    /// <summary>
    /// SP3001 rule analyzer - validates that exception message follows best practices.
    /// </summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class SP1001InvalidExceptionMessageAnalyzer : StyleCopPlusAnalyzer
    {
        public const string DiagnosticId = "SP1001";

        public const string Category = "Readability";

        public const string Title = "Follow exception message guidelines.";

        public const string MessageFormat =
            "Ensure that message is grammatically correct and that each sentence ends with a period";

        public const string Description = "The text of the Message property should completely describe the " +
            "error and, when possible, should also explain how to correct the error. Top-level exception " +
            "handlers may display the message to end-users, so you should ensure that it is grammatically " +
            "correct and that each sentence of the message ends with a period.";

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

        /// <summary>
        /// Registers analyzer actions for the specified compilation session using the specified settings.
        /// </summary>
        /// <param name="context">Analysis context to register actions in.</param>
        /// <param name="settings">Options for controlling the operation.</param>
        protected override void Register(CompilationStartAnalysisContext context, Settings settings)
        {
            context.RegisterSyntaxNodeAction(HandleSyntaxNode, SyntaxKind.ThrowStatement);
            context.RegisterSyntaxNodeAction(HandleSyntaxNode, SyntaxKind.ThrowExpression);
        }

        /// <summary>
        /// Look for exception creation syntax, find value of the "message" parameter (if any) and ensure
        /// that it ends with dot.
        /// </summary>
        /// <param name="context">Syntax node context for exception throw.</param>
        private static void HandleSyntaxNode(SyntaxNodeAnalysisContext context)
        {
            ObjectCreationExpressionSyntax node = null;
            if (context.Node is ThrowStatementSyntax statement)
                node = statement.Expression as ObjectCreationExpressionSyntax;
            else if (context.Node is ThrowExpressionSyntax expression)
                node = expression.Expression as ObjectCreationExpressionSyntax;

            if (node is null)
                return;

            SymbolInfo constructorSymbol = context.SemanticModel.GetSymbolInfo(node);
            IMethodSymbol constructor = constructorSymbol.Symbol as IMethodSymbol;
            if (constructor != null)
                AnalyzeExceptionConstructor(constructor, node, context);
        }

        private static void AnalyzeExceptionConstructor(
            IMethodSymbol constructor,
            ObjectCreationExpressionSyntax node,
            SyntaxNodeAnalysisContext context)
        {
            int messageParameterOrdinal = -1;
            for (int i = 0; i < constructor.Parameters.Length; i++)
            {
                if (constructor.Parameters[i].Name is "message")
                {
                    messageParameterOrdinal = i;
                    break;
                }
            }

            if (messageParameterOrdinal is -1) // Exception constructor does not have "message" parameter.
                return;

            // Find "message" parameter passed by name,
            ExpressionSyntax messageArgument =
                node.ArgumentList
                    .Arguments
                    .FirstOrDefault(a => a.NameColon?.Name.Identifier.Text == "message")?
                    .Expression;

            // If named parameter was not passed try to find it by index.
            if (messageArgument == null && node.ArgumentList.Arguments.Count > messageParameterOrdinal)
                messageArgument = node.ArgumentList.Arguments[messageParameterOrdinal].Expression;
            else
                return;

            Optional<object> messageValue = context.SemanticModel.GetConstantValue(messageArgument);
            if (!messageValue.HasValue)
                return;

            if (!messageValue.ToString().EndsWith("."))
                context.ReportDiagnostic(Diagnostic.Create(Rule, messageArgument.GetLocation()));
        }
    }
}
