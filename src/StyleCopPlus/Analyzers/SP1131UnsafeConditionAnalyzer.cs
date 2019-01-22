using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace StyleCopPlus.Analyzers
{
    /// <summary>
    /// SP1131 rule analyzer - validates that constants always come first in conditional operator.
    /// </summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class SP1131UnsafeConditionAnalyzer : StyleCopPlusAnalyzer
    {
        public const string DiagnosticId = "SP1131";

        public const string Category = "Readability";

        public const string Title = "Use safe conditions.";

        public const string MessageFormat =
            "Constant values should appear on the left-hand side of comparisons";

        public const string Description = "When a comparison is made between a variable and a literal, " +
            "the variable should be placed on the right-hand-side to avoid accidental assignment.";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            DiagnosticId,
            Title,
            MessageFormat,
            Category,
            DiagnosticSeverity.Warning,
            isEnabledByDefault: true,
            description: Description);

        private static readonly ImmutableArray<SyntaxKind> HandledBinaryExpressionKinds =
            ImmutableArray.Create(
                SyntaxKind.EqualsExpression,
                SyntaxKind.NotEqualsExpression);

        /// <summary>
        /// Gets a set of descriptors for the diagnostics that this analyzer is capable of producing.
        /// </summary>
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
        {
            get { return ImmutableArray.Create(Rule); }
        }

        /// <summary>
        /// Registers analyzer actions for the specified compilation session using the specified settings.
        /// </summary>
        /// <param name="context">Analysis context to register actions in.</param>
        /// <param name="settings">Options for controlling the operation.</param>
        protected override void Register(CompilationStartAnalysisContext context, Settings settings)
        {
            context.RegisterSyntaxNodeAction(HandleSyntaxNode, HandledBinaryExpressionKinds);
        }

        private static void HandleSyntaxNode(SyntaxNodeAnalysisContext context)
        {
            var binaryExpression = (BinaryExpressionSyntax)context.Node;
            var semanticModel = context.SemanticModel;

            if (IsLiteral(binaryExpression.Right, semanticModel) &&
                !IsLiteral(binaryExpression.Left, semanticModel))
            {
                context.ReportDiagnostic(Diagnostic.Create(Rule, binaryExpression.GetLocation()));
            }
        }

        private static bool IsLiteral(ExpressionSyntax expression, SemanticModel semanticModel)
        {
            // Default expressions are most of the time constants, but not for default(MyStruct).
            if (expression.IsKind(SyntaxKind.DefaultExpression))
                return true;

            var constantValue = semanticModel.GetConstantValue(expression);
            if (constantValue.HasValue)
                return true;

            var fieldSymbol = semanticModel.GetSymbolInfo(expression).Symbol as IFieldSymbol;
            if (null != fieldSymbol)
                return fieldSymbol.IsStatic && fieldSymbol.IsReadOnly;

            return false;
        }
    }
}
